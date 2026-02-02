namespace PulseX.Core.DTOs.Doctor
{
    public class DoctorDashboardDto
    {
        public int TotalPatients { get; set; }
        public int UpcomingAppointments { get; set; }
        public int TodayAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public decimal EstimatedEarnings { get; set; }
        public List<UpcomingAppointmentDto> NextAppointments { get; set; } = new List<UpcomingAppointmentDto>();
    }

    public class UpcomingAppointmentDto
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
