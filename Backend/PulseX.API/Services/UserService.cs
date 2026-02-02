using AutoMapper;
using PulseX.API.Helpers;
using PulseX.Core.DTOs.User;
using PulseX.Core.Enums;
using PulseX.Core.Interfaces;

namespace PulseX.API.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IHealthDataRepository _healthDataRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            IMedicalRecordRepository medicalRecordRepository,
            IHealthDataRepository healthDataRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _healthDataRepository = healthDataRepository;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> GetProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return _mapper.Map<UserProfileDto>(user);
        }

        public async Task<UserProfileDto> UpdateProfileAsync(int userId, UpdateProfileDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (!string.IsNullOrEmpty(dto.FullName))
                user.FullName = dto.FullName;
            
            if (!string.IsNullOrEmpty(dto.PhoneNumber))
                user.PhoneNumber = dto.PhoneNumber;

            await _userRepository.UpdateAsync(user);

            if (user.Role == UserRole.Patient && user.Patient != null)
            {
                var patient = user.Patient;
                
                if (dto.DateOfBirth.HasValue)
                    patient.DateOfBirth = dto.DateOfBirth;
                
                if (!string.IsNullOrEmpty(dto.Gender))
                    patient.Gender = dto.Gender;
                
                if (!string.IsNullOrEmpty(dto.Address))
                    patient.Address = dto.Address;
                
                if (!string.IsNullOrEmpty(dto.BloodType))
                    patient.BloodType = dto.BloodType;

                await _patientRepository.UpdateAsync(patient);
            }

            return await GetProfileAsync(userId);
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (!PasswordHelper.VerifyPassword(dto.CurrentPassword, user.PasswordHash))
            {
                throw new Exception("Current password is incorrect");
            }

            user.PasswordHash = PasswordHelper.HashPassword(dto.NewPassword);
            await _userRepository.UpdateAsync(user);
        }

        public async Task<PatientDashboardDto> GetPatientDashboardAsync(int patientUserId)
        {
            var patient = await _patientRepository.GetByUserIdAsync(patientUserId);
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }

            var appointments = await _appointmentRepository.GetByPatientIdAsync(patient.Id);
            var now = DateTime.UtcNow;
            
            var upcomingAppointments = appointments
                .Where(a => a.AppointmentDate > now && a.Status == AppointmentStatus.Scheduled)
                .OrderBy(a => a.AppointmentDate)
                .Take(5)
                .Select(a => new UpcomingAppointmentInfoDto
                {
                    Id = a.Id,
                    DoctorName = a.Doctor.User.FullName,
                    Specialization = a.Doctor.Specialization,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status.ToString()
                })
                .ToList();

            var medicalRecords = await _medicalRecordRepository.GetByPatientIdAsync(patient.Id);
            var healthData = await _healthDataRepository.GetByPatientIdAsync(patient.Id);
            
            var latestHealthData = healthData.OrderByDescending(h => h.RecordedAt).FirstOrDefault();
            var latestHealthMetric = latestHealthData != null 
                ? $"{latestHealthData.DataType}: {latestHealthData.Value} {latestHealthData.Unit}"
                : null;

            // Simple AI risk score calculation based on health data
            var aiRiskScore = CalculateAIRiskScore(healthData);

            return new PatientDashboardDto
            {
                UpcomingAppointments = appointments.Count(a => a.AppointmentDate > now && a.Status == AppointmentStatus.Scheduled),
                CompletedAppointments = appointments.Count(a => a.Status == AppointmentStatus.Completed),
                TotalMedicalRecords = medicalRecords.Count(),
                TotalHealthDataEntries = healthData.Count(),
                LatestHealthMetric = latestHealthMetric,
                NextAppointments = upcomingAppointments,
                AIRiskScore = aiRiskScore
            };
        }

        private string CalculateAIRiskScore(IEnumerable<Core.Models.HealthData> healthData)
        {
            if (!healthData.Any())
            {
                return "No data available";
            }

            // Simple risk assessment based on recent blood pressure readings
            var recentBP = healthData
                .Where(h => h.DataType.ToLower().Contains("blood") && h.DataType.ToLower().Contains("pressure"))
                .OrderByDescending(h => h.RecordedAt)
                .FirstOrDefault();

            if (recentBP != null)
            {
                var bpValue = recentBP.Value;
                if (bpValue.Contains("/"))
                {
                    var parts = bpValue.Split('/');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int systolic))
                    {
                        if (systolic > 140)
                            return "High Risk - Please consult your doctor";
                        else if (systolic > 130)
                            return "Moderate Risk - Monitor closely";
                        else
                            return "Low Risk - Keep monitoring";
                    }
                }
            }

            return "Normal - Continue healthy habits";
        }
    }
}
