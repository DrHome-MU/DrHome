using Microsoft.Identity.Client;

namespace Dr_Home.DTOs.AppointmentDTOs
{
    public class AppointmentIsDoneResponse
    {
        public string message { get; set; } = string.Empty;

        public bool IsDone { get; set; }
    }
}
