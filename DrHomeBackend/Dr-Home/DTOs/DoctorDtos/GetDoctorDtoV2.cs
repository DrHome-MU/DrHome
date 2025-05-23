using Dr_Home.DTOs.ReviewDtos;

namespace Dr_Home.DTOs.DoctorDtos
{
    public class GetDoctorDtoV2
    {
        public Guid doctorId { get; set; }

        public string doctorName { get; set; } = string.Empty;

        public string specialization { get; set; } = string.Empty;

        public string? profilePicPath { get; set; }

        public string? doctorSummary { get; set; }

      //  public IEnumerable<GetReviewDto> doctorReviews { get; set; } = [];

        public Guid clinicId { get; set; }

        public string clinicName { get; set; } = string.Empty;

        public string? clinicPhone { get; set; }

        public decimal appointmentFee { get; set; }

        public string? clinicCity { get; set; }

        public string? clinicRegion { get; set; }

        public string? detailedAddress { get; set; }

       // public IEnumerable<ScheduleResponse> schedules { get; set; } = [];
    }
}
