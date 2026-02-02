using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseX.API.Services;
using PulseX.Core.Enums;
using System.Security.Claims;

namespace PulseX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalRecordController : ControllerBase
    {
        private readonly MedicalRecordService _medicalRecordService;

        public MedicalRecordController(MedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> UploadMedicalRecord()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User ID not found in token" });

                if (!int.TryParse(userId, out int patientId))
                    return BadRequest(new { message = "Invalid user ID format" });

                var file = Request.Form.Files.FirstOrDefault();
                if (file == null)
                    return BadRequest(new { message = "No file provided" });

                var description = Request.Form["description"].FirstOrDefault();

                var record = await _medicalRecordService.UploadMedicalRecordAsync(patientId, file, description);
                return Ok(new { message = "Medical record uploaded successfully", data = record });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-records")]
        [Authorize]
        public async Task<IActionResult> GetMyRecords()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User ID not found in token" });

                if (!int.TryParse(userId, out int userIdInt))
                    return BadRequest(new { message = "Invalid user ID format" });

                var records = await _medicalRecordService.GetUserRecordsAsync(userIdInt, UserRole.Patient);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetPatientRecords(string patientId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User ID not found in token" });

                if (!int.TryParse(patientId, out int patientIdInt))
                    return BadRequest(new { message = "Invalid patient ID format" });

                if (!int.TryParse(userId, out int userIdInt))
                    return BadRequest(new { message = "Invalid user ID format" });

                var records = await _medicalRecordService.GetPatientRecordsByDoctorAsync(patientIdInt, userIdInt);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("download/{recordId}")]
        [Authorize]
        public async Task<IActionResult> DownloadRecord(string recordId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User ID not found in token" });

                if (!int.TryParse(recordId, out int recordIdInt))
                    return BadRequest(new { message = "Invalid record ID format" });

                if (!int.TryParse(userId, out int userIdInt))
                    return BadRequest(new { message = "Invalid user ID format" });

                var fileBytes = await _medicalRecordService.GetRecordFileAsync(recordIdInt, userIdInt, UserRole.Patient);
                if (fileBytes == null || fileBytes.Length == 0)
                    return NotFound(new { message = "Record not found" });

                return File(fileBytes, "application/octet-stream", $"record-{recordId}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
