using System.ComponentModel.DataAnnotations;

namespace Dr_Home.Data.Models
{
    public class Clinic
    {
        [Key]
        public Guid Id { get; set; }

        public string city { get; set; }

        public string region { get; set; }

        public string ClinicName { get; set; }

        [MaxLength(11)]
        [MinLength(11)]
        public string? PhoneNumber { get; set; }

        public List<Doctor_Schedule>? _schedules { get; set; }
    }
}
