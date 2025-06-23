using Dr_Home.Data;
using Dr_Home.Data.Models;
using Dr_Home.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dr_Home.Services.services
{
    public class ReviewService(AppDbContext db) : IReviewService
    {
        public async Task<Review> AddAsync(Review review , CancellationToken cancellationToken = default)
        {
            await db.Set<Review>().AddAsync(review ,cancellationToken);

            return review;

        }

        public async Task<Review> DeleteAsync(Review review)
        {
            db.Remove(review);
            return review;
        }

        public async Task<IEnumerable<Review>> GetAll()
        {
            var reviews = await db.Set<Review>().Include(r => r.patient).ToListAsync();   
            return reviews;
        }

        public async Task<IEnumerable<Review>> GetDoctorReviews(Guid DoctorId, CancellationToken cancellationToken = default)
        {
           var reviews = await db.Set<Review>().Include(r=>r.patient).
                Where(r=>r.DoctorId == DoctorId).ToListAsync(cancellationToken);

            return reviews;
        }

        public async Task<IEnumerable<Review>> GetPatientReviews(Guid PatientId, CancellationToken cancellationToken = default)
        {
           var reviews = await db.Set<Review>().Include(r => r.patient).Where(x=>x.PatientId == PatientId).ToListAsync(cancellationToken);

            return reviews;
        }

        public async Task<IEnumerable<Review>> GetReportedReviews()
        {
            var reviews = await db.Set<Review>().Include(r => r.patient).Where(x => x.IsReported == true).ToListAsync(); 
            return reviews;
        }

        public async Task<Review> GetReviewById(Guid reviewId)
        {
            var review = await db.Set<Review>().Include(r => r.patient).Include(r => r.doctor)
                .FirstOrDefaultAsync(r => r.Id == reviewId);
            return review;
        }

        public async Task<bool> IsPatientReviewedBefore(Guid patientId, Guid DoctorId, CancellationToken cancellationToken = default)
        {
            return await db.Set<Review>().AnyAsync(r => r.PatientId == patientId && r.DoctorId == DoctorId , cancellationToken);
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            db.Update(review);
            return review;
        }
    }
}
