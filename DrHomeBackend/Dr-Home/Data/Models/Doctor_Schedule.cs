using System.ComponentModel.DataAnnotations;

namespace Dr_Home.Data.Models
{
    public class Doctor_Schedule
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ClinicId { get; set; }

        public string WorkDay { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public decimal Fee { get; set; }

        public Clinic? clinic { get; set; } 
    }
}
