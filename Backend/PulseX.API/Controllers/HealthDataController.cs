using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseX.API.Services;
using PulseX.Core.DTOs.HealthData;
using System.Security.Claims;

namespace PulseX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthDataController : ControllerBase
    {
        private readonly HealthDataService _healthDataService;

        public HealthDataController(HealthDataService healthDataService)
        {
            _healthDataService = healthDataService;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddHealthData([FromBody] CreateHealthDataDto healthDataDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User ID not found in token" });

                if (!int.TryParse(userId, out int patientId))
                    return BadRequest(new { message = "Invalid user ID format" });

                var healthData = await _healthDataService.AddHealthDataAsync(patientId, healthDataDto);
                return Ok(new { message = "Health data added successfully", data = healthData });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-health-data")]
        [Authorize]
        public async Task<IActionResult> GetMyHealthData()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User ID not found in token" });

                if (!int.TryParse(userId, out int patientId))
                    return BadRequest(new { message = "Invalid user ID format" });

                var healthData = await _healthDataService.GetPatientHealthDataAsync(patientId);
                return Ok(healthData);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
