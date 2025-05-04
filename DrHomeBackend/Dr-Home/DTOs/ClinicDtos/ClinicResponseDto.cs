using System.ComponentModel.DataAnnotations;

namespace Dr_Home.DTOs.ClinicDtos
{
    public class ClinicResponseDto
    {
       
        public Guid Id { get; set; }

        public Guid DoctorId { get; set; }

        public string city { get; set; }

        public string region { get; set; }

        public string ClinicName { get; set; }

       
        public string? PhoneNumber { get; set; }

        public decimal AppointmentFee { get; set; }

        public string DetailedAddress { get; set; } = string.Empty;
    }
}
