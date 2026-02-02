using AutoMapper;
using PulseX.Core.DTOs.HealthData;
using PulseX.Core.Interfaces;
using PulseX.Core.Models;

namespace PulseX.API.Services
{
    public class HealthDataService
    {
        private readonly IHealthDataRepository _healthDataRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public HealthDataService(
            IHealthDataRepository healthDataRepository,
            IPatientRepository patientRepository,
            IMapper mapper)
        {
            _healthDataRepository = healthDataRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<HealthDataDto> AddHealthDataAsync(int patientId, CreateHealthDataDto dto)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }

            var healthData = _mapper.Map<HealthData>(dto);
            healthData.PatientId = patientId;

            await _healthDataRepository.AddAsync(healthData);

            return _mapper.Map<HealthDataDto>(healthData);
        }

        public async Task<IEnumerable<HealthDataDto>> GetPatientHealthDataAsync(int patientId)
        {
            var healthData = await _healthDataRepository.GetByPatientIdAsync(patientId);
            return _mapper.Map<IEnumerable<HealthDataDto>>(healthData);
        }
    }
}
