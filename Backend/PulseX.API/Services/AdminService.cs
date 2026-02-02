using AutoMapper;
using PulseX.Core.DTOs.Admin;
using PulseX.Core.Enums;
using PulseX.Core.Interfaces;

namespace PulseX.API.Services
{
    public class AdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IMapper _mapper;

        public AdminService(
            IUserRepository userRepository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository,
            IAppointmentRepository appointmentRepository,
            IActivityLogRepository activityLogRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
            _activityLogRepository = activityLogRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserManagementDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserManagementDto>>(users);
        }

        public async Task<UserManagementDto> UpdateUserStatusAsync(int userId, UpdateUserStatusDto dto, int adminUserId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.IsActive = dto.IsActive;
            await _userRepository.UpdateAsync(user);

            await _activityLogRepository.AddAsync(new Core.Models.ActivityLog
            {
                UserId = adminUserId,
                Action = "Update User Status",
                EntityType = "User",
                EntityId = userId,
                Details = $"User {user.FullName} status changed to {(dto.IsActive ? "Active" : "Inactive")}"
            });

            return _mapper.Map<UserManagementDto>(user);
        }

        public async Task<IEnumerable<ActivityLogDto>> GetAllActivityLogsAsync()
        {
            var logs = await _activityLogRepository.GetAllAsync();
            var logDtos = new List<ActivityLogDto>();

            foreach (var log in logs)
            {
                var user = await _userRepository.GetByIdAsync(log.UserId);
                var dto = _mapper.Map<ActivityLogDto>(log);
                dto.UserName = user?.FullName ?? "Unknown";
                logDtos.Add(dto);
            }

            return logDtos;
        }

        public async Task<IEnumerable<ActivityLogDto>> GetUserActivityLogsAsync(int userId)
        {
            var logs = await _activityLogRepository.GetByUserIdAsync(userId);
            var user = await _userRepository.GetByIdAsync(userId);
            
            var logDtos = logs.Select(log =>
            {
                var dto = _mapper.Map<ActivityLogDto>(log);
                dto.UserName = user?.FullName ?? "Unknown";
                return dto;
            });

            return logDtos;
        }

        public async Task<IEnumerable<UserManagementDto>> GetPendingDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            var pendingDoctors = doctors.Where(d => !d.IsApproved).ToList();
            
            return pendingDoctors.Select(d => new UserManagementDto
            {
                Id = d.User.Id,
                Email = d.User.Email,
                FullName = d.User.FullName,
                Role = d.User.Role.ToString(),
                IsActive = d.User.IsActive,
                CreatedAt = d.User.CreatedAt
            }).ToList();
        }

        public async Task<UserManagementDto> ApproveDoctorAsync(int doctorId, int adminUserId, ApproveDoctorDto dto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            doctor.IsApproved = dto.IsApproved;
            if (dto.IsApproved)
            {
                doctor.ApprovedByAdminId = adminUserId;
                doctor.ApprovedAt = DateTime.UtcNow;
            }

            await _doctorRepository.UpdateAsync(doctor);

            await _activityLogRepository.AddAsync(new Core.Models.ActivityLog
            {
                UserId = adminUserId,
                Action = dto.IsApproved ? "Approve Doctor" : "Reject Doctor",
                EntityType = "Doctor",
                EntityId = doctorId,
                Details = dto.IsApproved 
                    ? $"Doctor {doctor.User.FullName} approved" 
                    : $"Doctor {doctor.User.FullName} rejected. Reason: {dto.RejectionReason}"
            });

            return _mapper.Map<UserManagementDto>(doctor.User);
        }

        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var doctors = await _doctorRepository.GetAllAsync();
            var patients = users.Where(u => u.Role == UserRole.Patient).ToList();
            
            var allAppointments = new List<Core.Models.Appointment>();
            foreach (var doctor in doctors)
            {
                var doctorAppointments = await _appointmentRepository.GetByDoctorIdAsync(doctor.Id);
                allAppointments.AddRange(doctorAppointments);
            }

            var now = DateTime.UtcNow;
            var todayAppointments = allAppointments.Count(a => a.AppointmentDate.Date == now.Date);
            var completedAppointments = allAppointments.Count(a => a.Status == AppointmentStatus.Completed);
            var cancelledAppointments = allAppointments.Count(a => a.Status == AppointmentStatus.Cancelled);
            
            var totalRevenue = allAppointments
                .Where(a => a.Status == AppointmentStatus.Completed && a.PaymentStatus == PaymentStatus.Paid)
                .Sum(a => doctors.FirstOrDefault(d => d.Id == a.DoctorId)?.ConsultationPrice ?? 0);

            var recentLogs = await _activityLogRepository.GetAllAsync();
            var recentActivities = recentLogs
                .OrderByDescending(l => l.Timestamp)
                .Take(10)
                .Select(l =>
                {
                    var user = users.FirstOrDefault(u => u.Id == l.UserId);
                    return new RecentActivityDto
                    {
                        Action = l.Action,
                        UserName = user?.FullName ?? "Unknown",
                        Timestamp = l.Timestamp
                    };
                })
                .ToList();

            return new AdminDashboardDto
            {
                TotalPatients = patients.Count,
                TotalDoctors = doctors.Count(),
                ApprovedDoctors = doctors.Count(d => d.IsApproved),
                PendingDoctors = doctors.Count(d => !d.IsApproved),
                TotalAppointments = allAppointments.Count,
                TodayAppointments = todayAppointments,
                CompletedAppointments = completedAppointments,
                CancelledAppointments = cancelledAppointments,
                TotalRevenue = totalRevenue,
                RecentActivities = recentActivities
            };
        }
    }
}
