using System.ComponentModel.DataAnnotations;

namespace Dr_Home.Data.Models
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }

        public Guid DoctorId { get; set; }

        public string? Comment { get; set; }

        public int rating { get; set; }

        public DateTime ReviewTime { get; set; }

        public Patient? patient { get; set; }

        public Doctor?  doctor { get; set; } 
    }
}
