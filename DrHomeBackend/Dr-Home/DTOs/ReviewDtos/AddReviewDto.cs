namespace Dr_Home.DTOs.ReviewDtos
{
    public class AddReviewDto
    {
        public Guid DoctorId { get; set; }

        public string? Comment { get; set; }

        public int Rating { get; set; }
    }
}
