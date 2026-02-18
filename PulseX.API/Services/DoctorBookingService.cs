using PulseX.Core.DTOs.Doctor;
using PulseX.Core.DTOs.Appointment;
using PulseX.Core.Interfaces;
using PulseX.Core.Models;
using PulseX.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace PulseX.API.Services
{
    public class DoctorBookingService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DoctorBookingService> _logger;

        public DoctorBookingService(
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository,
            IAppointmentRepository appointmentRepository,
            IUserRepository userRepository,
            ILogger<DoctorBookingService> logger)
        {
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        // 1?? Get Doctors List with Filtering & Pagination
        public async Task<DoctorListResponseDto> GetDoctorsListAsync(DoctorListRequestDto request)
        {
            var query = (await _doctorRepository.GetAllAsync())
                .Where(d => d.IsApproved && d.User != null && d.User.IsActive)
                .AsQueryable();

            // Search by name or specialization
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(d =>
                    d.User!.FullName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    d.Specialization.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by price range
            if (request.MinPrice.HasValue)
            {
                query = query.Where(d => d.ConsultationPrice >= request.MinPrice.Value);
            }
            if (request.MaxPrice.HasValue)
            {
                query = query.Where(d => d.ConsultationPrice <= request.MaxPrice.Value);
            }

            // Filter by rating
            if (request.MinRating.HasValue)
            {
                query = query.Where(d => d.AverageRating >= request.MinRating.Value);
            }

            // Filter by location
            if (!string.IsNullOrEmpty(request.Location))
            {
                query = query.Where(d =>
                    d.ClinicLocation != null &&
                    d.ClinicLocation.Contains(request.Location, StringComparison.OrdinalIgnoreCase));
            }

            // Calculate statistics before pagination
            var allDoctors = query.ToList();
            var statistics = new DoctorStatisticsDto
            {
                TotalDoctors = allDoctors.Count,
                TopRatedDoctors = allDoctors.Count(d => d.AverageRating >= 4.5m),
                ActiveNow = allDoctors.Count(d => IsActiveNow(d)) // Simulated
            };

            // Apply pagination
            var totalItems = allDoctors.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);
            
            var doctors = allDoctors
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(d => new DoctorCardDto
                {
                    Id = d.Id,
                    FullName = d.User!.FullName,
                    Specialization = d.Specialization,
                    ProfilePicture = d.ProfilePicture,
                    ClinicLocation = d.ClinicLocation,
                    AverageRating = d.AverageRating,
                    TotalRatings = d.TotalRatings,
                    ConsultationPrice = d.ConsultationPrice,
                    YearsOfExperience = d.YearsOfExperience,
                    IsActiveNow = IsActiveNow(d)
                })
                .ToList();

            return new DoctorListResponseDto
            {
                Statistics = statistics,
                Doctors = doctors,
                Pagination = new PaginationDto
                {
                    CurrentPage = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalPages = totalPages,
                    TotalItems = totalItems,
                    HasPrevious = request.PageNumber > 1,
                    HasNext = request.PageNumber < totalPages
                }
            };
        }

        // 2?? Get Doctor Profile with Chat Authorization
        public async Task<DoctorProfileDto> GetDoctorProfileAsync(int doctorId, int userId)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null || !doctor.IsApproved)
            {
                throw new Exception("Doctor not found or not approved");
            }

            // Check if patient has active chat with this doctor
            var canChat = await CheckChatAuthorizationAsync(userId, doctorId);

            return new DoctorProfileDto
            {
                Id = doctor.Id,
                FullName = doctor.User!.FullName,
                Email = doctor.User.Email,
                PhoneNumber = doctor.User.PhoneNumber,
                Specialization = doctor.Specialization,
                LicenseNumber = doctor.LicenseNumber,
                ConsultationPrice = doctor.ConsultationPrice,
                ClinicLocation = doctor.ClinicLocation,
                Bio = doctor.Bio,
                YearsOfExperience = doctor.YearsOfExperience,
                ProfilePicture = doctor.ProfilePicture,
                AverageRating = doctor.AverageRating,
                TotalRatings = doctor.TotalRatings,
                
                // ?? Profile Details
                Education = doctor.Education,
                ProfessionalExperience = doctor.ProfessionalExperience,
                Certifications = doctor.Certifications,
                Languages = doctor.Languages,
                AvailableHours = doctor.AvailableHours,
                
                // For backwards compatibility
                About = doctor.Bio ?? "Experienced cardiologist dedicated to providing comprehensive heart care.",
                Experience = $"{doctor.YearsOfExperience}+ years of experience in cardiovascular medicine and patient care.",
                
                CanChat = canChat
            };
        }

        // 3?? Get Available Time Slots for a Doctor
        public async Task<AvailableSlotsDto> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            // Get all appointments for this doctor on the selected date
            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctorId);
            var bookedSlots = appointments
                .Where(a => a.AppointmentDate.Date == date.Date && a.Status != AppointmentStatus.Cancelled)
                .Select(a => a.AppointmentDate.ToString("HH:mm"))
                .ToList();

            // Generate time slots (9 AM to 5 PM, every 30 minutes)
            var slots = new List<TimeSlotDto>();
            var startTime = new DateTime(date.Year, date.Month, date.Day, 9, 0, 0);
            var endTime = new DateTime(date.Year, date.Month, date.Day, 17, 0, 0);

            while (startTime < endTime)
            {
                var timeSlot = startTime.ToString("HH:mm");
                slots.Add(new TimeSlotDto
                {
                    Time = timeSlot,
                    IsAvailable = !bookedSlots.Contains(timeSlot)
                });
                startTime = startTime.AddMinutes(30);
            }

            return new AvailableSlotsDto
            {
                Date = date,
                AvailableSlots = slots
            };
        }

        // 4?? Create Appointment with Payment
        public async Task<BookingSummaryDto> CreateAppointmentAsync(int userId, CreateAppointmentDto dto)
        {
            // Get patient
            var patient = await _patientRepository.GetByUserIdAsync(userId);
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }

            // Get doctor
            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            if (doctor == null || !doctor.IsApproved)
            {
                throw new Exception("Doctor not found or not approved");
            }

            // Validate time slot availability
            var appointmentDateTime = dto.AppointmentDate.Date.Add(TimeSpan.Parse(dto.TimeSlot));
            var existingAppointment = (await _appointmentRepository.GetByDoctorIdAsync(dto.DoctorId))
                .FirstOrDefault(a => 
                    a.AppointmentDate == appointmentDateTime && 
                    a.Status != AppointmentStatus.Cancelled);

            if (existingAppointment != null)
            {
                throw new Exception("Time slot is already booked. Please select another time.");
            }

            // Determine payment status based on method
            var paymentStatus = dto.PaymentMethod == PaymentMethod.Online ? PaymentStatus.Paid : PaymentStatus.Pending;
            var appointmentStatus = dto.PaymentMethod == PaymentMethod.Online ? AppointmentStatus.Confirmed : AppointmentStatus.Scheduled;
            
            // Chat expiry logic
            DateTime? chatExpiryDate = null;
            string chatMessage = "";
            
            if (dto.PaymentMethod == PaymentMethod.Online)
            {
                // Online payment: Chat active immediately for 7 days
                chatExpiryDate = DateTime.UtcNow.AddDays(7);
                chatMessage = "Chat is now active! You can message the doctor for 7 days.";
            }
            else
            {
                // Cash payment: Chat will be activated by doctor after visit
                chatMessage = "Chat will be activated by the doctor after your clinic visit.";
            }

            // Create appointment
            var appointment = new Appointment
            {
                PatientId = patient.Id,
                DoctorId = dto.DoctorId,
                AppointmentDate = appointmentDateTime,
                Notes = dto.Notes,
                Status = appointmentStatus,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = paymentStatus,
                ChatExpiryDate = chatExpiryDate,
                IsVideoCallActive = false, // Will be activated within appointment time window
                CreatedAt = DateTime.UtcNow
            };

            await _appointmentRepository.AddAsync(appointment);

            _logger.LogInformation($"Appointment created: ID={appointment.Id}, Patient={patient.Id}, Doctor={dto.DoctorId}, ChatExpiry={chatExpiryDate}");

            // Return booking summary
            return new BookingSummaryDto
            {
                AppointmentId = appointment.Id,
                DoctorName = doctor.User!.FullName,
                Specialization = doctor.Specialization,
                AppointmentDate = appointmentDateTime,
                TimeSlot = dto.TimeSlot,
                Status = appointmentStatus.ToString(),
                PaymentStatus = paymentStatus.ToString(),
                ConsultationFee = doctor.ConsultationPrice,
                ChatExpiryDate = chatExpiryDate,
                CanChat = chatExpiryDate.HasValue && DateTime.UtcNow < chatExpiryDate.Value,
                IsVideoCallActive = false,
                ChatMessage = chatMessage
            };
        }

        // 5?? Activate Chat (Doctor completes appointment)
        public async Task<BookingSummaryDto> ActivateChatAsync(int doctorId, int appointmentId, int expiryDays = 7)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
            {
                throw new Exception("Appointment not found");
            }

            if (appointment.DoctorId != doctorId)
            {
                throw new Exception("Unauthorized: This appointment does not belong to you");
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new Exception("Cannot activate chat for cancelled appointment");
            }

            // Activate chat for specified days
            appointment.ChatExpiryDate = DateTime.UtcNow.AddDays(expiryDays);
            appointment.Status = AppointmentStatus.Completed;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _appointmentRepository.UpdateAsync(appointment);

            _logger.LogInformation($"Chat activated for Appointment {appointmentId} until {appointment.ChatExpiryDate}");

            var doctor = await _doctorRepository.GetByIdAsync(doctorId);

            return new BookingSummaryDto
            {
                AppointmentId = appointment.Id,
                DoctorName = doctor?.User?.FullName ?? "Unknown",
                Specialization = doctor?.Specialization ?? "",
                AppointmentDate = appointment.AppointmentDate,
                TimeSlot = appointment.AppointmentDate.ToString("HH:mm"),
                Status = appointment.Status.ToString(),
                PaymentStatus = appointment.PaymentStatus.ToString(),
                ConsultationFee = doctor?.ConsultationPrice ?? 0,
                ChatExpiryDate = appointment.ChatExpiryDate,
                CanChat = true,
                IsVideoCallActive = false,
                ChatMessage = $"Chat activated! Available until {appointment.ChatExpiryDate:yyyy-MM-dd HH:mm}"
            };
        }

        // 6?? Check Chat Authorization (with Expiry)
        public async Task<bool> CheckChatAuthorizationAsync(int userId, int doctorId)
        {
            var patient = await _patientRepository.GetByUserIdAsync(userId);
            if (patient == null)
            {
                return false;
            }

            return await CanChatWithDoctorAsync(patient.Id, doctorId);
        }

        // Helper: Check if patient can chat with doctor (with expiry check)
        private async Task<bool> CanChatWithDoctorAsync(int patientId, int doctorId)
        {
            var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId);
            
            // Patient can chat if:
            // 1. Has appointment with this doctor
            // 2. ChatExpiryDate is set and not expired
            // 3. Status is Confirmed or Completed
            return appointments.Any(a =>
                a.DoctorId == doctorId &&
                a.ChatExpiryDate.HasValue &&
                DateTime.UtcNow < a.ChatExpiryDate.Value &&
                (a.Status == AppointmentStatus.Confirmed || a.Status == AppointmentStatus.Completed));
        }

        // 7?? Check Video Call Availability
        public async Task<bool> IsVideoCallAvailableAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
            {
                return false;
            }

            // Video call available within 1 hour before and after appointment time
            var now = DateTime.UtcNow;
            var appointmentStart = appointment.AppointmentDate.AddHours(-1);
            var appointmentEnd = appointment.AppointmentDate.AddHours(1);

            return now >= appointmentStart && now <= appointmentEnd &&
                   appointment.Status != AppointmentStatus.Cancelled &&
                   appointment.ChatExpiryDate.HasValue &&
                   now < appointment.ChatExpiryDate.Value;
        }

        // 8?? Get Doctor by UserId (for Controller)
        public async Task<int?> GetDoctorIdByUserIdAsync(int userId)
        {
            var doctor = await _doctorRepository.GetByUserIdAsync(userId);
            return doctor?.Id;
        }

        // Helper: Simulate active status (can be replaced with real logic)
        private bool IsActiveNow(Doctor doctor)
        {
            // Simulate: Doctors with rating > 4.0 are "active"
            return doctor.AverageRating > 4.0m;
        }
    }
}
