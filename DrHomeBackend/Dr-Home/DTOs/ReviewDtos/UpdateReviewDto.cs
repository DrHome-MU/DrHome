namespace Dr_Home.DTOs.ReviewDtos
{
    public class UpdateReviewDto
    {
       public Guid PatientId { get; set; }

        public string? Comment { get; set; }

        public int rating { get; set; }
    }
}
