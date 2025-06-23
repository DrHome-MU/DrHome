using System.ComponentModel.DataAnnotations;

namespace Dr_Home.Data.Models
{
    public class Appointment
    {
        [Key]
        public Guid Id { get; set; }

        public DateOnly? AppointmentDate { get; set; }

        public TimeOnly AppointmentTime {  get; set; }


        public string PatientName { get; set; } = string.Empty;

        public string PatientPhoneNumber {  get; set; } = string.Empty;

        public bool IsActive { get; set; } = true; 

        public bool IsDone { get; set; } = false;

        public string AppointmentDetails { get; set; } = string.Empty; 
        public Guid? DoctorId { get; set; }

        public Doctor? _doctor { get; set; } 

        public Guid? PatientId { get; set; }

        public Patient? _patient { get; set; }

        public Guid? ScheduleId { get; set; }

        public Doctor_Schedule? _schedule { get; set; }
    }
}
