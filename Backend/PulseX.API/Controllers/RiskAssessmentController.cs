using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseX.API.Services;
using PulseX.Core.DTOs.RiskAssessment;
using PulseX.Core.Interfaces;
using System.Security.Claims;

namespace PulseX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Patient")]
    public class RiskAssessmentController : ControllerBase
    {
        private readonly RiskAssessmentService _riskAssessmentService;
        private readonly IUserRepository _userRepository;

        public RiskAssessmentController(
            RiskAssessmentService riskAssessmentService,
            IUserRepository userRepository)
        {
            _riskAssessmentService = riskAssessmentService;
            _userRepository = userRepository;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateRisk([FromBody] CreateHeartRiskAssessmentDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                // Get patient record from userId
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || user.Patient == null)
                {
                    return BadRequest(new { message = "Patient profile not found. Please complete your registration." });
                }

                var result = await _riskAssessmentService.CreateHeartRiskAssessmentAsync(user.Patient.Id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                // Get patient record from userId
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || user.Patient == null)
                {
                    return BadRequest(new { message = "Patient profile not found" });
                }

                var result = await _riskAssessmentService.GetLatestRiskAssessmentAsync(user.Patient.Id);
                if (result == null)
                {
                    return NotFound(new { message = "No risk assessment found" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                // Get patient record from userId
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || user.Patient == null)
                {
                    return BadRequest(new { message = "Patient profile not found" });
                }

                var results = await _riskAssessmentService.GetAllAssessmentsAsync(user.Patient.Id);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}/latest")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetPatientLatest(int patientId)
        {
            try
            {
                var result = await _riskAssessmentService.GetLatestRiskAssessmentAsync(patientId);
                if (result == null)
                {
                    return NotFound(new { message = "No risk assessment found for this patient" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
