using AutoMapper;
using PulseX.Core.DTOs.MedicalRecord;
using PulseX.Core.Enums;
using PulseX.Core.Interfaces;
using PulseX.Core.Models;

namespace PulseX.API.Services
{
    public class MedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        private readonly string _uploadFolder;

        public MedicalRecordService(
            IMedicalRecordRepository medicalRecordRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            IMapper mapper,
            IWebHostEnvironment environment)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
            _uploadFolder = Path.Combine(environment.ContentRootPath, "Uploads", "MedicalRecords");
            
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        public async Task<MedicalRecordDto> UploadMedicalRecordAsync(int patientId, IFormFile file, string? description)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }

            if (file == null || file.Length == 0)
            {
                throw new Exception("No file provided");
            }

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var record = new MedicalRecord
            {
                PatientId = patientId,
                FileName = file.FileName,
                FilePath = filePath,
                FileType = file.ContentType,
                FileSize = file.Length,
                Description = description
            };

            await _medicalRecordRepository.AddAsync(record);

            return _mapper.Map<MedicalRecordDto>(record);
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetPatientRecordsAsync(int patientId)
        {
            var records = await _medicalRecordRepository.GetByPatientIdAsync(patientId);
            return _mapper.Map<IEnumerable<MedicalRecordDto>>(records);
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetPatientRecordsByDoctorAsync(int patientId, int doctorUserId)
        {
            // Verify doctor has appointment with patient
            var doctor = await _doctorRepository.GetByUserIdAsync(doctorUserId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctor.Id);
            var hasAppointment = appointments.Any(a => a.PatientId == patientId);

            if (!hasAppointment)
            {
                throw new Exception("Doctor does not have an appointment with this patient");
            }

            var records = await _medicalRecordRepository.GetByPatientIdAsync(patientId);
            return _mapper.Map<IEnumerable<MedicalRecordDto>>(records);
        }

        public async Task<byte[]> GetRecordFileAsync(int recordId, int userId, UserRole role)
        {
            var record = await _medicalRecordRepository.GetByIdAsync(recordId);
            if (record == null)
            {
                throw new Exception("Record not found");
            }

            // Verify access rights
            if (role == UserRole.Patient)
            {
                var patient = await _patientRepository.GetByUserIdAsync(userId);
                if (patient == null || record.PatientId != patient.Id)
                {
                    throw new Exception("Unauthorized");
                }
            }
            else if (role == UserRole.Doctor)
            {
                var doctor = await _doctorRepository.GetByUserIdAsync(userId);
                if (doctor == null)
                {
                    throw new Exception("Unauthorized");
                }

                var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctor.Id);
                var hasAppointment = appointments.Any(a => a.PatientId == record.PatientId);

                if (!hasAppointment)
                {
                    throw new Exception("Unauthorized");
                }
            }

            return await System.IO.File.ReadAllBytesAsync(record.FilePath);
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetUserRecordsAsync(int userId, UserRole role)
        {
            if (role == UserRole.Patient)
            {
                var patient = await _patientRepository.GetByUserIdAsync(userId);
                if (patient == null)
                {
                    throw new Exception("Patient not found");
                }

                var records = await _medicalRecordRepository.GetByPatientIdAsync(patient.Id);
                return _mapper.Map<IEnumerable<MedicalRecordDto>>(records);
            }
            else
            {
                throw new Exception("Invalid role for fetching user records");
            }
        }
    }
}
