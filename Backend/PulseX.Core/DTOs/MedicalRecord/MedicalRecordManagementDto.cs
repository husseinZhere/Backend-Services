namespace PulseX.Core.DTOs.MedicalRecord
{
    public class UploadMedicalRecordDto
    {
        public string RecordType { get; set; } = string.Empty; // ECG or X-Ray
        public string? Notes { get; set; }
    }

    public class MedicalRecordListDto
    {
        public int Id { get; set; }
        public string RecordType { get; set; } = string.Empty; // ECG or X-Ray
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string? FileType { get; set; }
        public long FileSize { get; set; }
        public string FileSizeFormatted { get; set; } = string.Empty; // e.g., "2.5 MB"
        public string? Notes { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class MedicalRecordsSummaryDto
    {
        public int TotalRecords { get; set; }
        public int EcgRecords { get; set; }
        public int XRayRecords { get; set; }
        public DateTime? LastUpdated { get; set; }
        public List<MedicalRecordListDto> Records { get; set; } = new();
    }

    public class GenerateQRCodeDto
    {
        public string QRCodeData { get; set; } = string.Empty; // URL to view records
        public string QRCodeImageBase64 { get; set; } = string.Empty; // Base64 image
        public DateTime GeneratedAt { get; set; }
        public DateTime? LastRecordUpdate { get; set; }
        public int TotalRecords { get; set; }
    }
}
