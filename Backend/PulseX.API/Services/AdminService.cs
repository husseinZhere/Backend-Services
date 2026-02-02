using AutoMapper;
using PulseX.Core.DTOs.Admin;
using PulseX.Core.Interfaces;

namespace PulseX.API.Services
{
    public class AdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IMapper _mapper;

        public AdminService(
            IUserRepository userRepository,
            IActivityLogRepository activityLogRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _activityLogRepository = activityLogRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserManagementDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserManagementDto>>(users);
        }

        public async Task<UserManagementDto> UpdateUserStatusAsync(int userId, UpdateUserStatusDto dto, int adminUserId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.IsActive = dto.IsActive;
            await _userRepository.UpdateAsync(user);

            await _activityLogRepository.AddAsync(new Core.Models.ActivityLog
            {
                UserId = adminUserId,
                Action = "Update User Status",
                EntityType = "User",
                EntityId = userId,
                Details = $"User {user.FullName} status changed to {(dto.IsActive ? "Active" : "Inactive")}"
            });

            return _mapper.Map<UserManagementDto>(user);
        }

        public async Task<IEnumerable<ActivityLogDto>> GetAllActivityLogsAsync()
        {
            var logs = await _activityLogRepository.GetAllAsync();
            var logDtos = new List<ActivityLogDto>();

            foreach (var log in logs)
            {
                var user = await _userRepository.GetByIdAsync(log.UserId);
                var dto = _mapper.Map<ActivityLogDto>(log);
                dto.UserName = user?.FullName ?? "Unknown";
                logDtos.Add(dto);
            }

            return logDtos;
        }

        public async Task<IEnumerable<ActivityLogDto>> GetUserActivityLogsAsync(int userId)
        {
            var logs = await _activityLogRepository.GetByUserIdAsync(userId);
            var user = await _userRepository.GetByIdAsync(userId);
            
            var logDtos = logs.Select(log =>
            {
                var dto = _mapper.Map<ActivityLogDto>(log);
                dto.UserName = user?.FullName ?? "Unknown";
                return dto;
            });

            return logDtos;
        }
    }
}
