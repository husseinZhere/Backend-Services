using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseX.API.Services;
using PulseX.Core.DTOs.Story;
using System.Security.Claims;

namespace PulseX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly StoryService _storyService;

        public StoryController(StoryService storyService)
        {
            _storyService = storyService;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateStory([FromBody] CreateStoryDto storyDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User ID not found in token" });

                if (!int.TryParse(userId, out int patientId))
                    return BadRequest(new { message = "Invalid user ID format" });

                var story = await _storyService.CreateStoryAsync(patientId, storyDto);
                return Ok(new { message = "Story created successfully", data = story });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("published")]
        [Authorize]
        public async Task<IActionResult> GetPublishedStories()
        {
            try
            {
                var stories = await _storyService.GetPublishedStoriesAsync();
                return Ok(stories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-stories")]
        [Authorize]
        public async Task<IActionResult> GetMyStories()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User ID not found in token" });

                if (!int.TryParse(userId, out int patientId))
                    return BadRequest(new { message = "Invalid user ID format" });

                var stories = await _storyService.GetPatientStoriesAsync(patientId);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStories()
        {
            try
            {
                var stories = await _storyService.GetAllStoriesAsync();
                return Ok(stories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("publish/{storyId}")]
        [Authorize]
        public async Task<IActionResult> PublishStory(string storyId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User ID not found in token" });

                if (!int.TryParse(storyId, out int storyIdInt))
                    return BadRequest(new { message = "Invalid story ID format" });

                if (!int.TryParse(userId, out int userIdInt))
                    return BadRequest(new { message = "Invalid user ID format" });

                var story = await _storyService.PublishStoryAsync(storyIdInt, userIdInt);
                return Ok(new { message = "Story published successfully", data = story });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("hide/{storyId}")]
        [Authorize]
        public async Task<IActionResult> HideStory(string storyId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User ID not found in token" });

                if (!int.TryParse(storyId, out int storyIdInt))
                    return BadRequest(new { message = "Invalid story ID format" });

                if (!int.TryParse(userId, out int userIdInt))
                    return BadRequest(new { message = "Invalid user ID format" });

                var story = await _storyService.HideStoryAsync(storyIdInt, userIdInt);
                return Ok(new { message = "Story hidden successfully", data = story });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("delete/{storyId}")]
        [Authorize]
        public async Task<IActionResult> DeleteStory(string storyId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User ID not found in token" });

                if (!int.TryParse(storyId, out int storyIdInt))
                    return BadRequest(new { message = "Invalid story ID format" });

                if (!int.TryParse(userId, out int userIdInt))
                    return BadRequest(new { message = "Invalid user ID format" });

                await _storyService.DeleteStoryAsync(storyIdInt, userIdInt);
                return Ok(new { message = "Story deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
