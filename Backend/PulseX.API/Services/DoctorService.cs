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
        private readonly IMapper _mapper;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IDoctorRatingRepository ratingRepository,
            IPatientRepository patientRepository,
            IAppointmentRepository appointmentRepository,
            IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _ratingRepository = ratingRepository;
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
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

            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctor.Id);
            var now = DateTime.UtcNow;
            
            var upcomingAppointments = appointments
                .Where(a => a.AppointmentDate > now && a.Status == AppointmentStatus.Scheduled)
                .OrderBy(a => a.AppointmentDate)
                .Take(5)
                .Select(a => new UpcomingAppointmentDto
                {
                    Id = a.Id,
                    PatientName = a.Patient.User.FullName,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status.ToString()
                })
                .ToList();

            var totalPatients = appointments.Select(a => a.PatientId).Distinct().Count();
            var completedAppointments = appointments.Count(a => a.Status == AppointmentStatus.Completed);
            var estimatedEarnings = appointments
                .Where(a => a.Status == AppointmentStatus.Completed && a.PaymentStatus == PaymentStatus.Paid)
                .Sum(a => doctor.ConsultationPrice);

            return new DoctorDashboardDto
            {
                TotalPatients = totalPatients,
                UpcomingAppointments = appointments.Count(a => a.AppointmentDate > now && a.Status == AppointmentStatus.Scheduled),
                TodayAppointments = appointments.Count(a => a.AppointmentDate.Date == now.Date && a.Status == AppointmentStatus.Scheduled),
                CompletedAppointments = completedAppointments,
                AverageRating = doctor.AverageRating,
                TotalRatings = doctor.TotalRatings,
                EstimatedEarnings = estimatedEarnings,
                NextAppointments = upcomingAppointments
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
    }
}
