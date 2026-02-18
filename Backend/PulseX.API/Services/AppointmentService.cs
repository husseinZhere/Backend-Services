using AutoMapper;
using PulseX.Core.DTOs.Appointment;
using PulseX.Core.Enums;
using PulseX.Core.Interfaces;
using PulseX.Core.Models;

namespace PulseX.API.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IActivityLogRepository activityLogRepository,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _activityLogRepository = activityLogRepository;
            _mapper = mapper;
        }

        public async Task<AppointmentDto> BookAppointmentAsync(int patientId, CreateAppointmentDto dto)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            var appointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                Notes = dto.Notes,
                PaymentMethod = dto.PaymentMethod,
                Status = AppointmentStatus.Scheduled,
                PaymentStatus = PaymentStatus.Pending
            };

            await _appointmentRepository.AddAsync(appointment);

            await _activityLogRepository.AddAsync(new ActivityLog
            {
                UserId = patient.UserId,
                Action = "Book Appointment",
                EntityType = "Appointment",
                EntityId = appointment.Id,
                Details = $"Appointment booked with Dr. {doctor.User.FullName}"
            });

            var result = await _appointmentRepository.GetByIdAsync(appointment.Id);
            return _mapper.Map<AppointmentDto>(result);
        }

        public async Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsAsync(int patientId)
        {
            var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsAsync(int doctorId)
        {
            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctorId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto> UpdateAppointmentStatusAsync(int appointmentId, UpdateAppointmentStatusDto dto, int userId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
            {
                throw new Exception("Appointment not found");
            }

            appointment.Status = dto.Status;
            
            if (dto.PaymentStatus.HasValue)
            {
                appointment.PaymentStatus = dto.PaymentStatus.Value;
            }

            await _appointmentRepository.UpdateAsync(appointment);

            await _activityLogRepository.AddAsync(new ActivityLog
            {
                UserId = userId,
                Action = "Update Appointment Status",
                EntityType = "Appointment",
                EntityId = appointment.Id,
                Details = $"Status changed to {dto.Status}"
            });

            var result = await _appointmentRepository.GetByIdAsync(appointmentId);
            return _mapper.Map<AppointmentDto>(result);
        }
    }
}
