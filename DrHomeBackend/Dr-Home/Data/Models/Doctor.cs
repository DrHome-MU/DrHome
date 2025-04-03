using System.ComponentModel.DataAnnotations.Schema;

namespace Dr_Home.Data.Models
{
    public class Doctor:User
    {
        
        public string? Summary { get; set; }

        public string? ProfilePic_Path { get; set; }


        public int SpecializationId { get; set; }

        public Specialization _specialization { get; set; } = default!;

        public List<Review>? Reviews { get; set; }

        public List<Clinic> ? clinics { get; set; }

        public List<Appointment>? _appointments { get; set; }
        

    }
}
