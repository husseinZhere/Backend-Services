using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseX.API.Services;
using PulseX.Core.DTOs.Doctor;
using System.Security.Claims;

namespace PulseX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorService _doctorService;

        public DoctorController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim!);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors([FromQuery] bool includeUnapproved = false)
        {
            try
            {
                // Only admins can see unapproved doctors
                var isAdmin = User.IsInRole("Admin");
                var approvedOnly = !includeUnapproved || !isAdmin;
                
                var doctors = await _doctorService.GetAllDoctorsAsync(approvedOnly);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorProfile(int id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorProfileAsync(id);
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/ratings")]
        public async Task<IActionResult> GetDoctorRatings(int id)
        {
            try
            {
                var ratings = await _doctorService.GetDoctorRatingsAsync(id);
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("rate")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> RateDoctor([FromBody] SubmitRatingDto dto)
        {
            try
            {
                var userId = GetUserId();
                var result = await _doctorService.SubmitRatingAsync(userId, dto);
                return Ok(new { message = "Rating submitted successfully", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("dashboard")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorDashboard()
        {
            try
            {
                var userId = GetUserId();
                var dashboard = await _doctorService.GetDoctorDashboardAsync(userId);
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
