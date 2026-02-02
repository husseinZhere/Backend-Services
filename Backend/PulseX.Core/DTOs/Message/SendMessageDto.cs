namespace PulseX.Core.DTOs.Message
{
    public class SendMessageDto
    {
        public int AppointmentId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
