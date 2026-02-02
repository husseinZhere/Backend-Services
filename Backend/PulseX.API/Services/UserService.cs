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
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
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
    }
}
