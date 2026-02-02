using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseX.API.Services;
using System.Security.Claims;

namespace PulseX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly ChatbotService _chatbotService;

        public ChatbotController(ChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost("chat")]
        [Authorize]
        public async Task<IActionResult> Chat([FromBody] string message)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User ID not found in token" });

                if (!int.TryParse(userId, out int patientId))
                    return BadRequest(new { message = "Invalid user ID format" });

                var response = await _chatbotService.GetChatbotResponseAsync(patientId, message);
                return Ok(new { message = "Response generated successfully", data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
