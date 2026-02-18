namespace PulseX.Core.DTOs.Doctor
{
    /// <summary>
    /// Doctor profile as seen by patients (in Doctor Profile Page)
    /// </summary>
    public class DoctorProfileDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Specialization { get; set; } = string.Empty;
        public string? LicenseNumber { get; set; }
        public decimal ConsultationPrice { get; set; }
        public string? ClinicLocation { get; set; }
        public string? Bio { get; set; }
        public int YearsOfExperience { get; set; }
        public string? ProfilePicture { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalRatings { get; set; }
        
        // ?? Profile Details (from Settings)
        public string? Education { get; set; }
        public string? ProfessionalExperience { get; set; } // JSON format
        public string? Certifications { get; set; }
        public string? Languages { get; set; }
        public string? AvailableHours { get; set; }
        
        // For backwards compatibility (can be removed later)
        public string About { get; set; } = string.Empty;
        public string Experience { get; set; } = string.Empty;
        
        // Chat authorization
        public bool CanChat { get; set; }
    }
}
