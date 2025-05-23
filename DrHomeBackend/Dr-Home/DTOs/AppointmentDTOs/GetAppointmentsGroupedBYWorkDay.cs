namespace Dr_Home.DTOs.AppointmentDTOs
{
    public class GetAppointmentsGroupedBYWorkDay
    {
        public DateOnly WorkDay { get; set; }

        public IEnumerable<AppointmentResponse> Appointments { get; set; } = [];
    }
}
