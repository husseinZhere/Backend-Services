namespace PulseX.Core.DTOs.Doctor
{
    /// <summary>
    /// Detailed patient profile for doctor (when doctor clicks "View Record")
    /// </summary>
    public class DoctorPatientProfileDto
    {
        // Patient Info
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string RiskLevel { get; set; } = string.Empty;
        
        // Vital Signs
        public VitalSignDto? HeartRate { get; set; }
        public VitalSignDto? BloodPressure { get; set; }
        public VitalSignDto? BloodSugar { get; set; }
        public VitalSignDto? Cholesterol { get; set; }
        public VitalSignDto? BloodCount { get; set; }
        
        // Medical Records
        public List<MedicalRecordItemDto> MedicalRecords { get; set; } = new();
        
        // QR Code Info
        public string? QRCodeData { get; set; }
        public DateTime? QRCodeGeneratedAt { get; set; }
        public int TotalFilesCount { get; set; }
    }

    public class VitalSignDto
    {
        public string Value { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public DateTime? LastUpdated { get; set; }
    }

    public class MedicalRecordItemDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string RecordType { get; set; } = string.Empty; // "Blood Test", "Radiology", "ECG"
        public DateTime UploadDate { get; set; }
        public string? FilePath { get; set; }
    }
}
