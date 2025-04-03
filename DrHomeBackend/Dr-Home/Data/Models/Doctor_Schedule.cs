using System.ComponentModel.DataAnnotations;

namespace Dr_Home.Data.Models
{
    public class Doctor_Schedule
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ClinicId { get; set; }

        public DateOnly WorkDay { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public int AppointmentDurationInMiniutes { get; set; }

        public decimal Fee { get; set; }

        public Clinic? clinic { get; set; } 

        public List<Appointment>? _appointments { get; set; }
    }
}
