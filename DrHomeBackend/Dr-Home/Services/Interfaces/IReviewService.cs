using Dr_Home.Data.Models;

namespace Dr_Home.Services.Interfaces
{
    public interface IReviewService
    {
        Task<Review> AddAsync(Review review , CancellationToken cancellationToken = default);

        Task<Review> GetReviewById(Guid reviewId);

        Task<Review> UpdateAsync (Review review);

        Task<Review> DeleteAsync (Review review);

        Task<bool>IsPatientReviewedBefore(Guid patientId , Guid DoctorId , CancellationToken cancellationToken = default);


        Task<IEnumerable<Review>> GetDoctorReviews(Guid DoctorId , CancellationToken cancellationToken = default);

        Task<IEnumerable<Review>>GetPatientReviews(Guid PatientId , CancellationToken cancellationToken = default);

        Task<IEnumerable<Review>> GetReportedReviews();
        Task<IEnumerable<Review>> GetAll();
        
    }
}
