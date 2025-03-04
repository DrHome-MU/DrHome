using System.ComponentModel.DataAnnotations.Schema;

namespace Dr_Home.Data.Models
{
    public class Doctor:User
    {
        
        public string? Summary { get; set; }

        public string? ProfilePic_Path { get; set; }

        public decimal? ConsultationFee { get; set; }


        public decimal? MedicalVisitFee { get; set; }

        public int? SpecializationId { get; set; }

        public Specialization? _specialization {  get; set; }    

        public List<Review>? Reviews { get; set; }

        public List<Doctor_Schedule> ? _schedules { get; set; }
        

    }
}
