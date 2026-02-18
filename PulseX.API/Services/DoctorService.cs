using AutoMapper;
using PulseX.Core.DTOs.Doctor;
using PulseX.Core.Enums;
using PulseX.Core.Interfaces;
using PulseX.Core.Models;

namespace PulseX.API.Services
{
    public class DoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDoctorRatingRepository _ratingRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHealthDataRepository _healthDataRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IRiskAssessmentRepository _riskAssessmentRepository;
        private readonly IMapper _mapper;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IDoctorRatingRepository ratingRepository,
            IPatientRepository patientRepository,
            IAppointmentRepository appointmentRepository,
            IUserRepository userRepository,
            IHealthDataRepository healthDataRepository,
            IMedicalRecordRepository medicalRecordRepository,
            IRiskAssessmentRepository riskAssessmentRepository,
            IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _ratingRepository = ratingRepository;
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _healthDataRepository = healthDataRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _riskAssessmentRepository = riskAssessmentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorListDto>> GetAllDoctorsAsync(bool approvedOnly = true)
        {
            var doctors = await _doctorRepository.GetAllAsync();
            
            if (approvedOnly)
            {
                doctors = doctors.Where(d => d.IsApproved).ToList();
            }
            
            return _mapper.Map<IEnumerable<DoctorListDto>>(doctors);
        }

        public async Task<DoctorProfileDto> GetDoctorProfileAsync(int doctorId)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            return _mapper.Map<DoctorProfileDto>(doctor);
        }

        public async Task<DoctorDashboardDto> GetDoctorDashboardAsync(int doctorUserId)
        {
            var doctor = await _doctorRepository.GetByUserIdAsync(doctorUserId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            var now = DateTime.UtcNow;
            
            // Get all appointments for this doctor
            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctor.Id);
            
            // Get unique patients from appointments
            var uniquePatientIds = appointments.Select(a => a.PatientId).Distinct().ToList();
            var totalPatients = uniquePatientIds.Count;
            
            // Get Critical Patients based on Risk Assessment (NOT just appointments)
            var criticalPatients = new List<CriticalPatientDto>();
            foreach (var patientId in uniquePatientIds)
            {
                var patient = await _patientRepository.GetByIdAsync(patientId);
                if (patient == null) continue;
                
                // Get latest risk assessment
                var riskAssessments = await _riskAssessmentRepository.GetByPatientIdAsync(patientId);
                var latestRisk = riskAssessments.OrderByDescending(r => r.AssessedAt).FirstOrDefault();
                
                if (latestRisk != null && latestRisk.RiskScore >= 70) // High Risk threshold
                {
                    var age = DateTime.Now.Year - patient.User.DateOfBirth.Year;
                    if (DateTime.Now < patient.User.DateOfBirth.AddYears(age)) age--;
                    
                    var lastAppointment = appointments
                        .Where(a => a.PatientId == patientId)
                        .OrderByDescending(a => a.AppointmentDate)
                        .FirstOrDefault();
                    
                    criticalPatients.Add(new CriticalPatientDto
                    {
                        PatientId = patient.Id,
                        PatientName = patient.User.FullName,
                        ProfilePicture = null,
                        Age = age,
                        RiskLevel = latestRisk.RiskScore >= 85 ? "High Risk" : "Moderate",
                        RiskScore = latestRisk.RiskScore,
                        LastCondition = latestRisk.Summary ?? latestRisk.RiskLevel,
                        LastVisit = lastAppointment?.AppointmentDate,
                        StatusBadge = latestRisk.RiskScore >= 85 ? "High Risk" : "Moderate"
                    });
                }
            }
            
            // Upcoming Appointments
            var upcomingAppointments = appointments
                .Where(a => a.AppointmentDate > now && a.Status == AppointmentStatus.Scheduled)
                .OrderBy(a => a.AppointmentDate)
                .Take(5)
                .Select(a => new UpcomingAppointmentDto
                {
                    Id = a.Id,
                    PatientName = a.Patient.User.FullName,
                    PatientProfilePicture = null,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status.ToString(),
                    AppointmentTime = a.AppointmentDate.ToString("hh:mm tt")
                })
                .ToList();
            
            // Today's Appointments
            var todayAppointments = appointments
                .Count(a => a.AppointmentDate.Date == now.Date && a.Status == AppointmentStatus.Scheduled);
            
            // Completed Appointments
            var completedAppointments = appointments
                .Count(a => a.Status == AppointmentStatus.Completed);
            
            // Estimated Earnings
            var estimatedEarnings = appointments
                .Where(a => a.Status == AppointmentStatus.Completed && a.PaymentStatus == PaymentStatus.Paid)
                .Sum(a => doctor.ConsultationPrice);
            
            // Recent Messages (last 3 unread or recent)
            var recentMessages = new List<RecentMessageDto>();
            foreach (var appointment in appointments.OrderByDescending(a => a.AppointmentDate).Take(10))
            {
                var messages = appointment.Messages
                    .Where(m => m.SenderId != doctorUserId) // Messages from patients
                    .OrderByDescending(m => m.SentAt)
                    .Take(1);
                
                foreach (var msg in messages)
                {
                    var timeSince = DateTime.UtcNow - msg.SentAt;
                    string timeAgo;
                    if (timeSince.TotalMinutes < 60)
                        timeAgo = $"{(int)timeSince.TotalMinutes} min ago";
                    else if (timeSince.TotalHours < 24)
                        timeAgo = $"{(int)timeSince.TotalHours} hour ago";
                    else
                        timeAgo = $"{(int)timeSince.TotalDays} day ago";
                    
                    recentMessages.Add(new RecentMessageDto
                    {
                        MessageId = msg.Id,
                        PatientId = appointment.PatientId,
                        PatientName = appointment.Patient.User.FullName,
                        PatientProfilePicture = null,
                        MessagePreview = msg.Content.Length > 50 
                            ? msg.Content.Substring(0, 50) + "..." 
                            : msg.Content,
                        SentAt = msg.SentAt,
                        IsUnread = !msg.IsRead,
                        TimeAgo = timeAgo
                    });
                }
            }
            recentMessages = recentMessages.OrderByDescending(m => m.SentAt).Take(3).ToList();
            
            // Weekly Overview (last 7 days)
            var weeklyOverview = new WeeklyOverviewDto();
            var startOfWeek = now.AddDays(-7);
            
            for (int i = 0; i < 7; i++)
            {
                var day = startOfWeek.AddDays(i);
                var dayAppointments = appointments
                    .Where(a => a.AppointmentDate.Date == day.Date)
                    .ToList();
                
                weeklyOverview.DailyStats.Add(new DailyStatsDto
                {
                    Day = day.ToString("ddd"), // "Mon", "Tue", etc.
                    PatientsCount = dayAppointments.Select(a => a.PatientId).Distinct().Count(),
                    AppointmentsCount = dayAppointments.Count
                });
            }
            
            weeklyOverview.TotalPatientsThisWeek = appointments
                .Where(a => a.AppointmentDate >= startOfWeek)
                .Select(a => a.PatientId)
                .Distinct()
                .Count();
            
            weeklyOverview.TotalAppointmentsThisWeek = appointments
                .Count(a => a.AppointmentDate >= startOfWeek);
            
            // Calculate change percentage (compared to previous week)
            var previousWeekStart = startOfWeek.AddDays(-7);
            var previousWeekAppointments = appointments
                .Count(a => a.AppointmentDate >= previousWeekStart && a.AppointmentDate < startOfWeek);
            
            if (previousWeekAppointments > 0)
            {
                weeklyOverview.ChangePercentage = 
                    ((decimal)(weeklyOverview.TotalAppointmentsThisWeek - previousWeekAppointments) / previousWeekAppointments) * 100;
            }

            return new DoctorDashboardDto
            {
                TotalPatients = totalPatients,
                CriticalCases = criticalPatients.Count,
                UpcomingAppointments = appointments.Count(a => a.AppointmentDate > now && a.Status == AppointmentStatus.Scheduled),
                TodayAppointments = todayAppointments,
                CompletedAppointments = completedAppointments,
                AverageRating = doctor.AverageRating,
                TotalRatings = doctor.TotalRatings,
                EstimatedEarnings = estimatedEarnings,
                NextAppointments = upcomingAppointments,
                CriticalPatients = criticalPatients.OrderByDescending(p => p.RiskScore).Take(5).ToList(),
                RecentMessages = recentMessages,
                WeeklyOverview = weeklyOverview
            };
        }

        public async Task<DoctorRatingDto> SubmitRatingAsync(int patientUserId, SubmitRatingDto dto)
        {
            // Validate rating value
            if (dto.Rating < 1 || dto.Rating > 5)
            {
                throw new Exception("Rating must be between 1 and 5");
            }

            // Get patient
            var patient = await _patientRepository.GetByUserIdAsync(patientUserId);
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }

            // Check if appointment exists and belongs to patient
            var appointment = await _appointmentRepository.GetByIdAsync(dto.AppointmentId);
            if (appointment == null)
            {
                throw new Exception("Appointment not found");
            }

            if (appointment.PatientId != patient.Id)
            {
                throw new Exception("This appointment does not belong to you");
            }

            // Check if appointment is completed
            if (appointment.Status != AppointmentStatus.Completed)
            {
                throw new Exception("You can only rate completed appointments");
            }

            // Check if already rated
            if (await _ratingRepository.HasRatedAppointmentAsync(dto.AppointmentId))
            {
                throw new Exception("You have already rated this appointment");
            }

            // Create rating
            var rating = new DoctorRating
            {
                DoctorId = appointment.DoctorId,
                PatientId = patient.Id,
                AppointmentId = dto.AppointmentId,
                Rating = dto.Rating,
                Review = dto.Review
            };

            await _ratingRepository.AddAsync(rating);

            // Update doctor average rating
            var doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorId);
            if (doctor != null)
            {
                var allRatings = await _ratingRepository.GetByDoctorIdAsync(doctor.Id);
                doctor.TotalRatings = allRatings.Count();
                doctor.AverageRating = (decimal)allRatings.Average(r => r.Rating);
                await _doctorRepository.UpdateAsync(doctor);
            }

            return new DoctorRatingDto
            {
                Id = rating.Id,
                DoctorId = rating.DoctorId,
                PatientId = rating.PatientId,
                PatientName = patient.User.FullName,
                Rating = rating.Rating,
                Review = rating.Review,
                CreatedAt = rating.CreatedAt
            };
        }

        public async Task<IEnumerable<DoctorRatingDto>> GetDoctorRatingsAsync(int doctorId)
        {
            var ratings = await _ratingRepository.GetByDoctorIdAsync(doctorId);
            return ratings.Select(r => new DoctorRatingDto
            {
                Id = r.Id,
                DoctorId = r.DoctorId,
                PatientId = r.PatientId,
                PatientName = r.Patient.User.FullName,
                Rating = r.Rating,
                Review = r.Review,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        /// <summary>
        /// Get Doctor's own profile (for Settings page)
        /// </summary>
        public async Task<DoctorSettingsProfileDto> GetDoctorSettingsProfileAsync(int userId)
        {
            var doctor = await _doctorRepository.GetByUserIdAsync(userId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            return new DoctorSettingsProfileDto
            {
                Id = doctor.Id,
                FullName = doctor.User!.FullName,
                Email = doctor.User.Email,
                PhoneNumber = doctor.User.PhoneNumber,
                ProfilePicture = doctor.ProfilePicture,
                Specialization = doctor.Specialization,
                LicenseNumber = doctor.LicenseNumber,
                ConsultationPrice = doctor.ConsultationPrice,
                ClinicLocation = doctor.ClinicLocation,
                Bio = doctor.Bio,
                YearsOfExperience = doctor.YearsOfExperience,
                Education = doctor.Education,
                ProfessionalExperience = doctor.ProfessionalExperience,
                Certifications = doctor.Certifications,
                Languages = doctor.Languages,
                AvailableHours = doctor.AvailableHours,
                AverageRating = doctor.AverageRating,
                TotalRatings = doctor.TotalRatings,
                IsApproved = doctor.IsApproved
            };
        }

        /// <summary>
        /// Update Doctor's profile (in Settings page)
        /// </summary>
        public async Task<DoctorSettingsProfileDto> UpdateDoctorSettingsProfileAsync(int userId, UpdateDoctorProfileDto dto)
        {
            var doctor = await _doctorRepository.GetByUserIdAsync(userId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            doctor.Specialization = dto.Specialization;
            doctor.LicenseNumber = dto.LicenseNumber;
            doctor.ConsultationPrice = dto.ConsultationPrice;
            doctor.ClinicLocation = dto.ClinicLocation;
            doctor.Bio = dto.Bio;
            doctor.YearsOfExperience = dto.YearsOfExperience;
            doctor.Education = dto.Education;
            doctor.ProfessionalExperience = dto.ProfessionalExperience;
            doctor.Certifications = dto.Certifications;
            doctor.Languages = dto.Languages;
            doctor.AvailableHours = dto.AvailableHours;

            await _doctorRepository.UpdateAsync(doctor);

            return await GetDoctorSettingsProfileAsync(userId);
        }

        /// <summary>
        /// Upload Doctor's profile picture
        /// </summary>
        public async Task<string> UpdateDoctorProfilePictureAsync(int userId, string filePath)
        {
            var doctor = await _doctorRepository.GetByUserIdAsync(userId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            doctor.ProfilePicture = filePath;
            await _doctorRepository.UpdateAsync(doctor);

            return filePath;
        }

        /// <summary>
        /// Get appointments that need rating (for patient)
        /// </summary>
        public async Task<IEnumerable<PendingRatingDto>> GetPendingRatingsAsync(int patientUserId)
        {
            var patient = await _patientRepository.GetByUserIdAsync(patientUserId);
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }

            var appointments = await _appointmentRepository.GetByPatientIdAsync(patient.Id);
            var pendingRatings = new List<PendingRatingDto>();

            foreach (var appointment in appointments)
            {
                // Only completed appointments that haven't been rated
                if (appointment.Status == AppointmentStatus.Completed)
                {
                    var alreadyRated = await _ratingRepository.HasRatedAppointmentAsync(appointment.Id);
                    if (!alreadyRated)
                    {
                        var doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorId);
                        if (doctor != null)
                        {
                            pendingRatings.Add(new PendingRatingDto
                            {
                                AppointmentId = appointment.Id,
                                DoctorId = doctor.Id,
                                DoctorName = doctor.User!.FullName,
                                DoctorSpecialization = doctor.Specialization,
                                DoctorProfilePicture = doctor.ProfilePicture,
                                AppointmentDate = appointment.AppointmentDate,
                                CompletedDate = appointment.UpdatedAt ?? appointment.AppointmentDate
                            });
                        }
                    }
                }
            }

            return pendingRatings.OrderByDescending(p => p.CompletedDate);
        }

        /// <summary>
        /// Get list of patients for doctor (Patient List page)
        /// </summary>
        public async Task<IEnumerable<DoctorPatientListItemDto>> GetDoctorPatientsAsync(int doctorUserId)
        {
            var doctor = await _doctorRepository.GetByUserIdAsync(doctorUserId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctor.Id);
            
            // Get unique patients from appointments
            var patientIds = appointments.Select(a => a.PatientId).Distinct();
            var patientsList = new List<DoctorPatientListItemDto>();

            foreach (var patientId in patientIds)
            {
                var patient = await _patientRepository.GetByIdAsync(patientId);
                if (patient == null) continue;

                var patientAppointments = appointments.Where(a => a.PatientId == patientId).ToList();
                var lastAppointment = patientAppointments.OrderByDescending(a => a.AppointmentDate).FirstOrDefault();

                // Calculate age
                var age = DateTime.Now.Year - patient.User.DateOfBirth.Year;
                if (DateTime.Now < patient.User.DateOfBirth.AddYears(age)) age--;

                // Determine visit type (from last appointment)
                var visitType = lastAppointment?.PaymentMethod == Core.Enums.PaymentMethod.Cash ? "Clinic" : "Online";

                // Chat status
                string chatStatus;
                bool canChat = false;
                if (lastAppointment != null && lastAppointment.ChatExpiryDate.HasValue)
                {
                    var timeLeft = lastAppointment.ChatExpiryDate.Value - DateTime.UtcNow;
                    if (timeLeft.TotalDays > 0)
                    {
                        if (timeLeft.TotalDays < 1)
                            chatStatus = "1 day left";
                        else
                            chatStatus = $"{(int)timeLeft.TotalDays} days left";
                        canChat = true;
                    }
                    else if (timeLeft.TotalHours > 0 && timeLeft.TotalHours <= 1)
                    {
                        chatStatus = "Open Chat";
                        canChat = true;
                    }
                    else
                    {
                        chatStatus = "Expired";
                    }
                }
                else
                {
                    chatStatus = "No chat";
                }

                // Risk level (from latest risk assessment)
                var riskAssessments = await _riskAssessmentRepository.GetByPatientIdAsync(patientId);
                var latestRisk = riskAssessments.OrderByDescending(r => r.AssessedAt).FirstOrDefault();
                string riskLevel = "Unknown";
                if (latestRisk != null)
                {
                    if (latestRisk.RiskScore < 30)
                        riskLevel = "Low";
                    else if (latestRisk.RiskScore < 70)
                        riskLevel = "Moderate";
                    else
                        riskLevel = "High";
                }

                // Last visit relative time
                string lastVisitRelative = "Never";
                if (lastAppointment != null)
                {
                    var daysSince = (DateTime.UtcNow - lastAppointment.AppointmentDate).TotalDays;
                    if (daysSince < 1)
                        lastVisitRelative = "Today";
                    else if (daysSince < 7)
                        lastVisitRelative = $"{(int)daysSince} days ago";
                    else if (daysSince < 30)
                        lastVisitRelative = $"{(int)(daysSince / 7)} weeks ago";
                    else
                        lastVisitRelative = $"{(int)(daysSince / 30)} months ago";
                }

                patientsList.Add(new DoctorPatientListItemDto
                {
                    PatientId = patient.Id,
                    PatientName = patient.User.FullName,
                    ProfilePicture = null, // Add if you have profile pictures
                    Age = age,
                    Gender = patient.Gender?.ToString() ?? "Unknown",
                    VisitType = visitType,
                    ChatStatus = chatStatus,
                    CanChat = canChat,
                    RiskLevel = riskLevel,
                    LastVisit = lastAppointment?.AppointmentDate,
                    LastVisitRelative = lastVisitRelative
                });
            }

            return patientsList.OrderByDescending(p => p.LastVisit);
        }

        /// <summary>
        /// Get detailed patient profile for doctor (when doctor clicks "View Record")
        /// </summary>
        public async Task<DoctorPatientProfileDto> GetPatientProfileForDoctorAsync(int doctorUserId, int patientId)
        {
            var doctor = await _doctorRepository.GetByUserIdAsync(doctorUserId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            // Verify doctor has appointment with this patient
            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctor.Id);
            var hasAppointment = appointments.Any(a => a.PatientId == patientId);
            if (!hasAppointment)
            {
                throw new Exception("You don't have access to this patient's records");
            }

            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }

            // Calculate age
            var age = DateTime.Now.Year - patient.User.DateOfBirth.Year;
            if (DateTime.Now < patient.User.DateOfBirth.AddYears(age)) age--;

            // Get latest health data
            var healthData = await _healthDataRepository.GetByPatientIdAsync(patientId);
            var latestHeartRate = healthData.Where(h => h.DataType == "HeartRate").OrderByDescending(h => h.RecordedAt).FirstOrDefault();
            var latestBloodPressure = healthData.Where(h => h.DataType == "BloodPressure").OrderByDescending(h => h.RecordedAt).FirstOrDefault();
            var latestBloodSugar = healthData.Where(h => h.DataType == "BloodSugar").OrderByDescending(h => h.RecordedAt).FirstOrDefault();
            var latestCholesterol = healthData.Where(h => h.DataType == "Cholesterol").OrderByDescending(h => h.RecordedAt).FirstOrDefault();
            var latestBloodCount = healthData.Where(h => h.DataType == "BloodCount").OrderByDescending(h => h.RecordedAt).FirstOrDefault();

            // Get medical records
            var medicalRecords = await _medicalRecordRepository.GetByPatientIdAsync(patientId);
            var recordItems = medicalRecords.Select(mr => new MedicalRecordItemDto
            {
                Id = mr.Id,
                FileName = mr.RecordType,
                RecordType = mr.RecordType,
                UploadDate = mr.UploadedAt,
                FilePath = mr.FilePath
            }).ToList();

            // Risk level
            var riskAssessments = await _riskAssessmentRepository.GetByPatientIdAsync(patientId);
            var latestRisk = riskAssessments.OrderByDescending(r => r.AssessedAt).FirstOrDefault();
            string riskLevel = "Unknown";
            if (latestRisk != null)
            {
                if (latestRisk.RiskScore < 30)
                    riskLevel = "Low";
                else if (latestRisk.RiskScore < 70)
                    riskLevel = "Moderate";
                else
                    riskLevel = "High";
            }

            return new DoctorPatientProfileDto
            {
                PatientId = patient.Id,
                PatientName = patient.User.FullName,
                ProfilePicture = null,
                Age = age,
                Gender = patient.Gender?.ToString() ?? "Unknown",
                RiskLevel = riskLevel,
                HeartRate = latestHeartRate != null ? new VitalSignDto { Value = latestHeartRate.Value, Unit = "bpm", LastUpdated = latestHeartRate.RecordedAt } : null,
                BloodPressure = latestBloodPressure != null ? new VitalSignDto { Value = latestBloodPressure.Value, Unit = "mmHg", LastUpdated = latestBloodPressure.RecordedAt } : null,
                BloodSugar = latestBloodSugar != null ? new VitalSignDto { Value = latestBloodSugar.Value, Unit = "mg/dL", LastUpdated = latestBloodSugar.RecordedAt } : null,
                Cholesterol = latestCholesterol != null ? new VitalSignDto { Value = latestCholesterol.Value, Unit = "mg/dL", LastUpdated = latestCholesterol.RecordedAt } : null,
                BloodCount = latestBloodCount != null ? new VitalSignDto { Value = latestBloodCount.Value, Unit = "g/dL", LastUpdated = latestBloodCount.RecordedAt } : null,
                MedicalRecords = recordItems,
                QRCodeData = patient.QRCodeData,
                QRCodeGeneratedAt = patient.QRCodeGeneratedAt,
                TotalFilesCount = recordItems.Count
            };
        }

        /// <summary>
        /// Add/Update patient's medical data (doctor only)
        /// </summary>
        public async Task AddPatientMedicalDataAsync(int doctorUserId, int patientId, AddPatientMedicalDataDto dto)
        {
            var doctor = await _doctorRepository.GetByUserIdAsync(doctorUserId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            // Verify doctor has appointment with this patient
            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctor.Id);
            var hasAppointment = appointments.Any(a => a.PatientId == patientId);
            if (!hasAppointment)
            {
                throw new Exception("You don't have access to add data for this patient");
            }

            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }

            // Add health data entries
            var healthDataList = new List<Core.Models.HealthData>();

            if (dto.HeartRate != null)
            {
                healthDataList.Add(new Core.Models.HealthData
                {
                    PatientId = patientId,
                    DataType = "HeartRate",
                    Value = dto.HeartRate,
                    RecordedAt = DateTime.UtcNow
                });
            }

            if (dto.BloodPressure != null)
            {
                healthDataList.Add(new Core.Models.HealthData
                {
                    PatientId = patientId,
                    DataType = "BloodPressure",
                    Value = dto.BloodPressure,
                    RecordedAt = DateTime.UtcNow
                });
            }

            if (dto.BloodSugar.HasValue)
            {
                healthDataList.Add(new Core.Models.HealthData
                {
                    PatientId = patientId,
                    DataType = "BloodSugar",
                    Value = dto.BloodSugar.Value.ToString(),
                    RecordedAt = DateTime.UtcNow
                });
            }

            if (dto.Cholesterol.HasValue)
            {
                healthDataList.Add(new Core.Models.HealthData
                {
                    PatientId = patientId,
                    DataType = "Cholesterol",
                    Value = dto.Cholesterol.Value.ToString(),
                    RecordedAt = DateTime.UtcNow
                });
            }

            if (dto.BloodCount != null)
            {
                healthDataList.Add(new Core.Models.HealthData
                {
                    PatientId = patientId,
                    DataType = "BloodCount",
                    Value = dto.BloodCount,
                    RecordedAt = DateTime.UtcNow
                });
            }

            // Save all health data
            foreach (var healthData in healthDataList)
            {
                await _healthDataRepository.AddAsync(healthData);
            }
        }
    }
}
