using System.ComponentModel.DataAnnotations;

namespace Dr_Home.DTOs.ClinicDtos
{
    public class UpdateClinicDto
    {
        public Guid DoctorId { get; set; }
        public required string city { get; set; }

        public required string region { get; set; }

        public required string ClinicName { get; set; }

        public string? PhoneNumber { get; set; }

        public decimal AppointmentFee { get; set; }

        public string DetailedAddress { get; set; } = string.Empty;
    }
}
