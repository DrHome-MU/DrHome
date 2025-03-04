using Dr_Home.Data.Models;
using Dr_Home.DTOs.ReviewDtos;
using Dr_Home.Helpers.helpers;

namespace Dr_Home.Helpers.Interfaces
{
    public interface IReviewHelper
    {
        Task<ApiResponse<Review>> AddReview(Guid id, AddReviewDto dto);
        Task<ApiResponse<Review>>UpdateReview(Guid id , UpdateReviewDto dto);

        Task<ApiResponse<Review>> DeleteReview(Guid id);

        Task<ApiResponse<IEnumerable<GetReviewDto>>> GetDoctorReviews(Guid DoctorId);

        Task<ApiResponse<IEnumerable<GetReviewDto>>>GetPatientReviews(Guid PatientId);

        Task<ApiResponse<Decimal>>GetDoctorAverageRating(Guid DoctorId);
    }
}
