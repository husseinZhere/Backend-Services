namespace PulseX.Core.DTOs.Patient
{
    public class PatientDashboardDto
    {
        // User Info
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }

        // Latest Vitals
        public VitalCardDto? HeartRate { get; set; }
        public VitalCardDto? BloodPressure { get; set; }
        public VitalCardDto? BloodSugar { get; set; }
        public VitalCardDto? BloodCount { get; set; }

        // Latest Risk Assessment
        public RiskAssessmentSummaryDto? LatestRiskAssessment { get; set; }

        // Upcoming Appointments
        public List<UpcomingAppointmentDto> UpcomingAppointments { get; set; } = new();

        // Top Doctors
        public List<TopDoctorDto> TopDoctors { get; set; } = new();

        // Weekly Health Chart Data
        public WeeklyHealthDataDto? WeeklyHealthData { get; set; }
    }

    public class VitalCardDto
    {
        public string DataType { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Normal, High, Low
        public DateTime RecordedAt { get; set; }
        public string StatusColor { get; set; } = string.Empty; // green, yellow, red
    }

    public class RiskAssessmentSummaryDto
    {
        public int Id { get; set; }
        public decimal RiskScore { get; set; }
        public string RiskLevel { get; set; } = string.Empty; // Low, Medium, High
        public string Summary { get; set; } = string.Empty;
        public string Recommendation { get; set; } = string.Empty;
        public decimal Accuracy { get; set; } = 98.5m;
        public DateTime AssessedAt { get; set; }
    }

    public class UpcomingAppointmentDto
    {
        public int Id { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string? DoctorSpecialty { get; set; }
        public string? DoctorProfilePicture { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string TimeSlot { get; set; } = string.Empty;
    }

    public class TopDoctorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Specialty { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public decimal ConsultationPrice { get; set; }
        public string? ProfilePicture { get; set; }
    }

    public class WeeklyHealthDataDto
    {
        public List<DailyHealthPointDto> HeartRateData { get; set; } = new();
        public List<DailyHealthPointDto> BloodPressureData { get; set; } = new();
    }

    public class DailyHealthPointDto
    {
        public DateTime Date { get; set; }
        public string Value { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty; // Mon, Tue, etc.
    }
}
