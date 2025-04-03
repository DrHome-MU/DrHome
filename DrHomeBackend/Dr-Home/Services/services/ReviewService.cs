using Dr_Home.Data;
using Dr_Home.Data.Models;
using Dr_Home.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dr_Home.Services.services
{
    public class ReviewService(AppDbContext db) : IReviewService
    {
        public async Task<Review> AddAsync(Review review)
        {
            await db.Set<Review>().AddAsync(review);

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

        public async Task<IEnumerable<Review>> GetDoctorReviews(Guid DoctorId)
        {
           var reviews = await db.Set<Review>().Include(r=>r.patient).
                Where(r=>r.DoctorId == DoctorId).ToListAsync();

            return reviews;
        }

        public async Task<IEnumerable<Review>> GetPatientReviews(Guid PatientId)
        {
           var reviews = await db.Set<Review>().Include(r => r.patient).Where(x=>x.PatientId == PatientId).ToListAsync();

            return reviews;
        }

        public async Task<Review> GetReviewById(Guid reviewId)
        {
            var review = await db.Set<Review>().Include(r => r.patient).Include(r => r.doctor)
                .FirstOrDefaultAsync(r => r.Id == reviewId);
            return review;
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            db.Update(review);
            return review;
        }
    }
}
