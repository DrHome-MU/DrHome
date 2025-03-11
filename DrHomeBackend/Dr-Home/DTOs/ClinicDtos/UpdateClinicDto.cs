using System.ComponentModel.DataAnnotations;

namespace Dr_Home.DTOs.ClinicDtos
{
    public class UpdateClinicDto
    {
        public Guid ClinicId { get; set; }
        public required string city { get; set; }

        public required string region { get; set; }

        public required string ClinicName { get; set; }


        [MaxLength(11)]
        [MinLength(11)]
        public string? PhoneNumber { get; set; }
    }
}
