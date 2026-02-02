namespace PulseX.Core.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? BloodType { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public ICollection<HealthData> HealthData { get; set; } = new List<HealthData>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<Story> Stories { get; set; } = new List<Story>();
        public ICollection<DoctorRating> DoctorRatings { get; set; } = new List<DoctorRating>();
    }
}
