namespace Dr_Home.DTOs.ReviewDtos
{
    public class GetReviewDto
    {
        public string ReviwerName { get; set; }

        public string? Comment { get; set; }

        public int rating { get; set; }

        public DateTime ReviewTime { get; set; }
    }
}
