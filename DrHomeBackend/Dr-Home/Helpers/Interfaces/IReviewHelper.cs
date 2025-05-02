using Dr_Home.Data.Models;
using Dr_Home.DTOs.ReviewDtos;
using Dr_Home.Helpers.helpers;

namespace Dr_Home.Helpers.Interfaces
{
    public interface IReviewHelper
    {
        Task<Result<GetReviewDto>> AddReview(AddReviewDto dto , CancellationToken cancellationToken = default);
        Task<Result>UpdateReview(Guid ReviewId , UpdateReviewDto dto , CancellationToken cancellationToken = default);

        Task<Result> DeleteReview(Guid id , CancellationToken cancellationToken = default);

        Task<Result<IEnumerable<GetReviewDto>>> GetDoctorReviews(Guid DoctorId , CancellationToken cancellationToken = default);

       

        Task<Result<GetAverageReviewDto>>GetDoctorAverageRating(Guid DoctorId , CancellationToken cancellationToken = default);

    }
}
