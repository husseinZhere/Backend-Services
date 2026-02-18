using AutoMapper;
using PulseX.Core.DTOs.Message;
using PulseX.Core.Enums;
using PulseX.Core.Interfaces;
using PulseX.Core.Models;

namespace PulseX.API.Services
{
    public class MessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public MessageService(
            IMessageRepository messageRepository,
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IMapper mapper)
        {
            _messageRepository = messageRepository;
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<MessageDto> SendMessageAsync(int userId, UserRole role, SendMessageDto dto)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(dto.AppointmentId);
            if (appointment == null)
            {
                throw new Exception("Appointment not found");
            }

            // Verify the user is part of this appointment
            if (role == UserRole.Patient)
            {
                var patient = await _patientRepository.GetByUserIdAsync(userId);
                if (patient == null || appointment.PatientId != patient.Id)
                {
                    throw new Exception("Unauthorized");
                }
            }
            else if (role == UserRole.Doctor)
            {
                var doctor = await _doctorRepository.GetByUserIdAsync(userId);
                if (doctor == null || appointment.DoctorId != doctor.Id)
                {
                    throw new Exception("Unauthorized");
                }
            }

            var message = new Message
            {
                AppointmentId = dto.AppointmentId,
                SenderId = userId,
                Content = dto.Content
            };

            await _messageRepository.AddAsync(message);

            // Load message with sender info for response
            var result = await _messageRepository.GetByIdAsync(message.Id);
            
            if (result == null)
            {
                throw new Exception("Failed to retrieve message after creation");
            }
            
            var messageDto = new MessageDto
            {
                Id = result.Id,
                AppointmentId = result.AppointmentId,
                SenderId = result.SenderId,
                Content = result.Content,
                SentAt = result.SentAt,
                IsRead = result.IsRead
            };

            // Get sender name based on role
            if (role == UserRole.Patient)
            {
                var patient = await _patientRepository.GetByUserIdAsync(userId);
                messageDto.SenderName = patient?.User.FullName ?? "Unknown";
            }
            else if (role == UserRole.Doctor)
            {
                var doctor = await _doctorRepository.GetByUserIdAsync(userId);
                messageDto.SenderName = doctor?.User.FullName ?? "Unknown";
            }

            return messageDto;
        }

        public async Task<IEnumerable<MessageDto>> GetAppointmentMessagesAsync(int appointmentId, int userId, UserRole role)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
            {
                throw new Exception("Appointment not found");
            }

            // Verify the user is part of this appointment
            if (role == UserRole.Patient)
            {
                var patient = await _patientRepository.GetByUserIdAsync(userId);
                if (patient == null || appointment.PatientId != patient.Id)
                {
                    throw new Exception("Unauthorized");
                }
            }
            else if (role == UserRole.Doctor)
            {
                var doctor = await _doctorRepository.GetByUserIdAsync(userId);
                if (doctor == null || appointment.DoctorId != doctor.Id)
                {
                    throw new Exception("Unauthorized");
                }
            }

            var messages = await _messageRepository.GetByAppointmentIdAsync(appointmentId);
            
            var messageDtos = new List<MessageDto>();
            
            foreach (var message in messages)
            {
                var messageDto = new MessageDto
                {
                    Id = message.Id,
                    AppointmentId = message.AppointmentId,
                    SenderId = message.SenderId,
                    Content = message.Content,
                    SentAt = message.SentAt,
                    IsRead = message.IsRead,
                    SenderName = "Unknown"
                };

                // Determine sender name
                var senderUser = await _messageRepository.GetByIdAsync(message.Id);
                if (senderUser != null)
                {
                    var patientCheck = await _patientRepository.GetByUserIdAsync(message.SenderId);
                    if (patientCheck != null)
                    {
                        messageDto.SenderName = patientCheck.User.FullName;
                    }
                    else
                    {
                        var doctorCheck = await _doctorRepository.GetByUserIdAsync(message.SenderId);
                        if (doctorCheck != null)
                        {
                            messageDto.SenderName = doctorCheck.User.FullName;
                        }
                    }
                }

                messageDtos.Add(messageDto);
            }

            return messageDtos;
        }
    }
}
