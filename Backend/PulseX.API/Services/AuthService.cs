using AutoMapper;
using PulseX.API.Helpers;
using PulseX.Core.DTOs.Auth;
using PulseX.Core.Enums;
using PulseX.Core.Interfaces;
using PulseX.Core.Models;

namespace PulseX.API.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly JwtHelper _jwtHelper;
        private readonly IMapper _mapper;

        public AuthService(
            IUserRepository userRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IActivityLogRepository activityLogRepository,
            JwtHelper jwtHelper,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _activityLogRepository = activityLogRepository;
            _jwtHelper = jwtHelper;
            _mapper = mapper;
        }

        public async Task<LoginResponseDto> RegisterPatientAsync(RegisterPatientDto dto)
        {
            if (await _userRepository.ExistsAsync(dto.Email))
            {
                throw new Exception("Email already exists");
            }

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = PasswordHelper.HashPassword(dto.Password),
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Role = UserRole.Patient
            };

            await _userRepository.AddAsync(user);

            var patient = new Patient
            {
                UserId = user.Id,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Address = dto.Address,
                BloodType = dto.BloodType
            };

            await _patientRepository.AddAsync(patient);

            await _activityLogRepository.AddAsync(new ActivityLog
            {
                UserId = user.Id,
                Action = "Patient Registration",
                Details = $"Patient {user.FullName} registered"
            });

            var token = _jwtHelper.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                UserId = user.Id
            };
        }

        public async Task<LoginResponseDto> CreateDoctorAsync(CreateDoctorDto dto)
        {
            if (await _userRepository.ExistsAsync(dto.Email))
            {
                throw new Exception("Email already exists");
            }

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = PasswordHelper.HashPassword(dto.Password),
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Role = UserRole.Doctor
            };

            await _userRepository.AddAsync(user);

            var doctor = new Doctor
            {
                UserId = user.Id,
                Specialization = dto.Specialization,
                LicenseNumber = dto.LicenseNumber,
                ConsultationPrice = dto.ConsultationPrice,
                ClinicLocation = dto.ClinicLocation,
                Bio = dto.Bio,
                YearsOfExperience = dto.YearsOfExperience
            };

            await _doctorRepository.AddAsync(doctor);

            var token = _jwtHelper.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                UserId = user.Id
            };
        }

        public async Task<LoginResponseDto> CreateAdminAsync(CreateAdminDto dto)
        {
            if (await _userRepository.ExistsAsync(dto.Email))
            {
                throw new Exception("Email already exists");
            }

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = PasswordHelper.HashPassword(dto.Password),
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Role = UserRole.Admin
            };

            await _userRepository.AddAsync(user);

            var token = _jwtHelper.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                UserId = user.Id
            };
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            
            if (user == null || !PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password");
            }

            if (!user.IsActive)
            {
                throw new Exception("Account is deactivated");
            }

            await _activityLogRepository.AddAsync(new ActivityLog
            {
                UserId = user.Id,
                Action = "Login",
                Details = $"User {user.FullName} logged in"
            });

            var token = _jwtHelper.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                UserId = user.Id
            };
        }
    }
}
