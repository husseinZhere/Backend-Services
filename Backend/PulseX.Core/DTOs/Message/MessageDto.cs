namespace PulseX.Core.DTOs.Message
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
