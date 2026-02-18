using System.ComponentModel.DataAnnotations;

namespace PulseX.Core.DTOs.Doctor
{
    /// <summary>
    /// DTO for Doctor to update their profile in Settings page
    /// </summary>
    public class UpdateDoctorProfileDto
    {
        [Required]
        [MaxLength(100)]
        public string Specialization { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? LicenseNumber { get; set; }
        
        [Range(0, 10000)]
        public decimal ConsultationPrice { get; set; }
        
        [MaxLength(200)]
        public string? ClinicLocation { get; set; }
        
        [MaxLength(1000)]
        public string? Bio { get; set; }
        
        [Range(0, 50)]
        public int YearsOfExperience { get; set; }
        
        // Profile Details
        [MaxLength(500)]
        public string? Education { get; set; }
        
        [MaxLength(2000)]
        public string? ProfessionalExperience { get; set; } // JSON format
        
        [MaxLength(1000)]
        public string? Certifications { get; set; }
        
        [MaxLength(100)]
        public string? Languages { get; set; }
        
        [MaxLength(200)]
        public string? AvailableHours { get; set; }
    }
}
