namespace Dr_Home.DTOs.ReviewDtos
{
    public class AddReviewDto
    {
        public Guid PatientId { get; set; }

        public Guid DoctorId { get; set; }

        public string? Comment { get; set; }

        public int rating { get; set; }
    }
}
