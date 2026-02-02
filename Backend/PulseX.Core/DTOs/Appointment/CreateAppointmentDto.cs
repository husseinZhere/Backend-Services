using PulseX.Core.Enums;

namespace PulseX.Core.DTOs.Appointment
{
    public class CreateAppointmentDto
    {
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Notes { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
