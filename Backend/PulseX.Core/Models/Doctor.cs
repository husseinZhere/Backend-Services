namespace PulseX.Core.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Specialization { get; set; } = string.Empty;
        public string? LicenseNumber { get; set; }
        public decimal ConsultationPrice { get; set; }
        public string? ClinicLocation { get; set; }
        public string? Bio { get; set; }
        public int YearsOfExperience { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
