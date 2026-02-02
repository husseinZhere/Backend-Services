namespace PulseX.Core.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        // Navigation properties
        public Appointment Appointment { get; set; } = null!;
        public Patient? SenderPatient { get; set; }
        public Doctor? SenderDoctor { get; set; }
    }
}
