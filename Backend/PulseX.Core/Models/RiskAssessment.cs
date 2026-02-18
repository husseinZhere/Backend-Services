namespace PulseX.Core.Models
{
    public class RiskAssessment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public decimal RiskScore { get; set; }
        public string RiskLevel { get; set; } = string.Empty; // Low, Medium, High
        public string? Summary { get; set; }
        public string? Recommendation { get; set; }
        public DateTime AssessedAt { get; set; } = DateTime.UtcNow;
        
        // Health Metrics (Professional Enums)
        public string CholesterolLevel { get; set; } = string.Empty; // normal, borderline, high
        public string SleepHours { get; set; } = string.Empty; // <6, 6-8, >8
        public string AlcoholConsumption { get; set; } = string.Empty; // low, medium, high
        public string PhysicalActivity { get; set; } = string.Empty; // low, medium, high
        
        // Medical History
        public bool PreviousHeartIssues { get; set; }
        public bool FamilyHistory { get; set; }

        // Navigation properties
        public Patient Patient { get; set; } = null!;
    }
}
