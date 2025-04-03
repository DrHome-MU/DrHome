namespace Dr_Home.Data.Models
{
    public class Patient:User
    {
        public List<Review> ? Reviews { get; set; }

        public List<Appointment>? _appointments { get; set; }
    }
}
