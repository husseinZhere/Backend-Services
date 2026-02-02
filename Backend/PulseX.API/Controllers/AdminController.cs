using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseX.API.Services;
using PulseX.Core.DTOs.Admin;
using System.Security.Claims;

namespace PulseX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _adminService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("users/{userId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserStatus(string userId, [FromBody] UpdateUserStatusDto statusDto)
        {
            try
            {
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(adminId))
                    return Unauthorized(new { message = "Admin ID not found in token" });

                if (!int.TryParse(userId, out int userIdInt))
                    return BadRequest(new { message = "Invalid user ID format" });

                if (!int.TryParse(adminId, out int adminIdInt))
                    return BadRequest(new { message = "Invalid admin ID format" });

                var user = await _adminService.UpdateUserStatusAsync(userIdInt, statusDto, adminIdInt);
                return Ok(new { message = "User status updated successfully", data = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("activity-logs")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllActivityLogs()
        {
            try
            {
                var logs = await _adminService.GetAllActivityLogsAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("activity-logs/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserActivityLogs(string userId)
        {
            try
            {
                if (!int.TryParse(userId, out int userIdInt))
                    return BadRequest(new { message = "Invalid user ID format" });

                var logs = await _adminService.GetUserActivityLogsAsync(userIdInt);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
