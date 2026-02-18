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
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IHealthDataRepository _healthDataRepository;
        private readonly JwtHelper _jwtHelper;
        private readonly IMapper _mapper;

        public AuthService(
          IUserRepository userRepository,
          IPatientRepository patientRepository,
          IActivityLogRepository activityLogRepository,
          IHealthDataRepository healthDataRepository,
          JwtHelper jwtHelper,
          IMapper mapper)
        {
            _userRepository = userRepository;
            _patientRepository = patientRepository;
            _activityLogRepository = activityLogRepository;
            _healthDataRepository = healthDataRepository;
            _jwtHelper = jwtHelper;
            _mapper = mapper;
        }

        public async Task<LoginResponseDto> RegisterPatientAsync(RegisterPatientDto dto)
        {
            // Validate email uniqueness
            if (await _userRepository.ExistsAsync(dto.Email))
            {
                throw new Exception("Email already exists");
            }

            // ✅ Use computed FullName from DTO
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = PasswordHelper.HashPassword(dto.Password),
                FullName = dto.FullName,  // Uses computed property
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Role = UserRole.Patient,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            var patientId = await GenerateUniquePatientIdAsync();

            var patient = new Patient
            {
                UserId = user.Id,
                PatientId = patientId,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                EmailNotificationsEnabled = true,
                DarkModeEnabled = false
            };

            await _patientRepository.AddAsync(patient);

            await _activityLogRepository.AddAsync(new ActivityLog
            {
                UserId = user.Id,
                Action = "Patient Registered",
                EntityType = "User",
                EntityId = user.Id,
                Details = $"New patient registered: {user.FullName} (PatientId: {patientId})"
            });

            var token = _jwtHelper.GenerateToken(user);

            // ✅ Split name for response
            var (firstName, lastName) = NameHelper.SplitFullName(user.FullName);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName,
                FirstName = firstName,
                LastName = lastName,
                Role = user.Role.ToString(),
                UserId = user.Id
            };
        }

        /// <summary>
        /// Generate unique PatientId in format: PAT001, PAT002, PAT003...
        /// </summary>
        private async Task<string> GenerateUniquePatientIdAsync()
        {
            // Get the last patient to determine next ID
            var lastPatient = await _patientRepository.GetLastPatientAsync();
            
            if (lastPatient == null)
            {
                return "PAT001";  // First patient
            }

            // Extract number from last PatientId (e.g., "PAT005" -> 5)
            var lastNumber = int.Parse(lastPatient.PatientId.Substring(3));
            var nextNumber = lastNumber + 1;

            // Format as PAT001, PAT002, etc.
            return $"PAT{nextNumber:D3}";
        }

        public async Task<LoginResponseDto> CreateAdminAsync(CreateAdminDto dto)
        {
            if (await _userRepository.ExistsAsync(dto.Email))
            {
                throw new Exception("Email already exists");
            }

            // ✅ Use computed FullName
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = PasswordHelper.HashPassword(dto.Password),
                FullName = dto.FullName,  // Uses computed property
                PhoneNumber = dto.PhoneNumber,
                Role = UserRole.Admin
            };

            await _userRepository.AddAsync(user);

            var token = _jwtHelper.GenerateToken(user);

            // ✅ Split name for response
            var (firstName, lastName) = NameHelper.SplitFullName(user.FullName);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName,
                FirstName = firstName,
                LastName = lastName,
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

            if (user.Role == UserRole.Doctor && user.Doctor != null)
            {
                if (!user.Doctor.IsApproved)
                {
                    throw new Exception("Your account is pending admin approval. Please wait for approval to access the system.");
                }
            }

            await _activityLogRepository.AddAsync(new ActivityLog
            {
                UserId = user.Id,
                Action = "Login",
                Details = $"User {user.FullName} logged in"
            });

            var token = _jwtHelper.GenerateToken(user);

            // ✅ Split name for response
            var (firstName, lastName) = NameHelper.SplitFullName(user.FullName);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName,
                FirstName = firstName,
                LastName = lastName,
                Role = user.Role.ToString(),
                UserId = user.Id
            };
        }
    }
}
