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
        
        // Approval fields
        public bool IsApproved { get; set; } = false;
        public int? ApprovedByAdminId { get; set; }
        public DateTime? ApprovedAt { get; set; }
        
        // Rating fields
        public decimal AverageRating { get; set; } = 0;
        public int TotalRatings { get; set; } = 0;

        // Navigation properties
        public User User { get; set; } = null!;
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<DoctorRating> Ratings { get; set; } = new List<DoctorRating>();
    }
}
