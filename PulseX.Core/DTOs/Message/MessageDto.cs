namespace PulseX.Core.DTOs.Message
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderRole { get; set; } = string.Empty; // Patient or Doctor
        public string Content { get; set; } = string.Empty;
        
        // Attachment fields
        public string? AttachmentPath { get; set; }
        public string? AttachmentType { get; set; }
        public long? AttachmentSize { get; set; }
        public string? AttachmentSizeFormatted { get; set; }
        
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }

    public class SendMessageDto
    {
        public int AppointmentId { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    public class ChatHistoryDto
    {
        public int AppointmentId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public DateTime? ChatExpiryDate { get; set; }
        public bool CanChat { get; set; }
        public bool IsVideoCallAvailable { get; set; }
        public List<MessageDto> Messages { get; set; } = new();
    }
}
