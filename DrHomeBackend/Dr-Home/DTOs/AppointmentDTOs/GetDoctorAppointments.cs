namespace Dr_Home.DTOs.AppointmentDTOs
{
    public class GetDoctorAppointments
    {
        public Guid ClinicId { get; set; }

        public string ClinicName { get; set; } = string.Empty;
        public DateOnly WorkDay { get; set; }

        public List<AppointmentResponse> Appointments { get; set; } = [];
    }
}
