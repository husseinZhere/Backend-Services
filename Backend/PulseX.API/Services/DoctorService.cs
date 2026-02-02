using AutoMapper;
using PulseX.Core.DTOs.Doctor;
using PulseX.Core.Interfaces;

namespace PulseX.API.Services
{
    public class DoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorListDto>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();
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
    }
}
