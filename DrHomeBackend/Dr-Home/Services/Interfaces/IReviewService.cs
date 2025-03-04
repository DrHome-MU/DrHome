using Dr_Home.Data.Models;

namespace Dr_Home.Services.Interfaces
{
    public interface IReviewService
    {
        Task<Review> AddAsync(Review review);

        Task<Review> GetReviewById(Guid reviewId);

        Task<Review> UpdateAsync (Review review);

        Task<Review> DeleteAsync (Review review);

        Task<IEnumerable<Review>> GetDoctorReviews(Guid DoctorId);

        Task<IEnumerable<Review>>GetPatientReviews(Guid PatientId);
        
    }
}
