namespace PulseX.Core.DTOs.Story
{
    public class CreateStoryDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
