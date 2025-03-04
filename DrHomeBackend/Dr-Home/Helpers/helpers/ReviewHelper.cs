using Dr_Home.Data.Models;
using Dr_Home.DTOs.ReviewDtos;
using Dr_Home.Helpers.Interfaces;
using Dr_Home.Services.Interfaces;
using Dr_Home.UnitOfWork;

namespace Dr_Home.Helpers.helpers
{
    public class ReviewHelper(IUnitOfWork _unitOfWork) : IReviewHelper
    {
        public async Task<ApiResponse<Review>> AddReview(Guid id, AddReviewDto dto)
        {
           
            var review = new Review
            {
                PatientId = id,
                DoctorId = dto.DoctorId,
                Comment = dto.Comment,
                rating = dto.Rating,
                ReviewTime = DateTime.Now
            };

            await _unitOfWork._reviewService.AddAsync(review);
            _unitOfWork.Complete(); 

            return new ApiResponse<Review>{
                Success = true ,
                Message = "Review Is Added Successfully", 
                Data = review
                
            };

        }

        public async Task<ApiResponse<Review>> DeleteReview(Guid id)
        {
            var review = await _unitOfWork._reviewService.GetReviewById(id);

            if (review == null)
            {
                return new ApiResponse<Review>
                {
                    Success = false ,
                    Message = "Review Already Removed"
                };
            }

            if (review.patient.Id == id)
            {
                return new ApiResponse<Review>
                {
                    Success = false ,
                    Message = "Unauthorized User"
                };
            }

            await _unitOfWork._reviewService.DeleteAsync(review);
            _unitOfWork.Complete();

            return new ApiResponse<Review>
            {
                Success = true,
                Message = "Deleted Successfully."
            };
        }

        public async Task<ApiResponse<decimal>> GetDoctorAverageRating(Guid DoctorId)
        {
           var reviews = await _unitOfWork._reviewService.GetDoctorReviews(DoctorId);

            if(reviews == null)
            {
                return new ApiResponse<decimal> { Success = false, Message = "There Is No Reviews" };

            }

            var sumRating = reviews.Sum(r => r.rating);

            int reviewsCount = reviews.Count();

            decimal averageRating = (decimal) sumRating / reviewsCount;

            return new ApiResponse<decimal>
            {
                Success = true,
                Message = "Average Rating Calculated Successfully",
                Data = averageRating
            };

        }

        public async Task<ApiResponse<IEnumerable<GetReviewDto>>> GetDoctorReviews(Guid DoctorId)
        {
           var reviews = await _unitOfWork._reviewService.GetDoctorReviews(DoctorId);

            Console.WriteLine(reviews.Count()); 
           
            if(reviews == null || reviews.Count() == 0)
            {
                return new ApiResponse<IEnumerable<GetReviewDto>>
                {
                    Success = false,
                    Message = "There Is No Reviews For this Doctor",
                };
            }

            List<GetReviewDto> result = new List<GetReviewDto>();

            foreach(var review in reviews)
            {
                var dto = new GetReviewDto
                {
                    ReviwerName = review.patient.FullName , 
                    Comment = review.Comment, 
                    rating = review.rating,
                    ReviewTime = review.ReviewTime
                };
                result.Add(dto);
            }

            return new ApiResponse<IEnumerable<GetReviewDto>>
            {
                Success = true,
                Message = "Loading Reviews Is Done Successfully",
                Data = result
            };

        }

        public async Task<ApiResponse<IEnumerable<GetReviewDto>>> GetPatientReviews(Guid PatientId)
        {
            var reviews = await _unitOfWork._reviewService.GetPatientReviews(PatientId);

            if (reviews == null || reviews.Count() == 0)
            {
                return new ApiResponse<IEnumerable<GetReviewDto>>
                {
                    Success = false,
                    Message = "This User didn`t Review any doctor",
                };
            }

            List<GetReviewDto> result = new List<GetReviewDto>();

            foreach (var review in reviews)
            {
                var dto = new GetReviewDto
                {
                    ReviwerName = review.patient.FullName,
                    Comment = review.Comment,
                    rating = review.rating,
                    ReviewTime = review.ReviewTime
                };
                result.Add(dto);
            }

            return new ApiResponse<IEnumerable<GetReviewDto>>
            {
                Success = true,
                Message = "Loading Reviews Is Done Successfully",
                Data = result
            };
        }

        public async  Task<ApiResponse<Review>> UpdateReview(Guid id, UpdateReviewDto dto)
        {
            var review = await _unitOfWork._reviewService.GetReviewById(dto.ReviewId);

            if (review == null)
            {
                return new ApiResponse<Review>
                {
                    Success = false,
                    Message = "Review Doesn`t Exist",
                    Data = null
                };
            }

            if(review.patient.Id != id) {
                return new ApiResponse<Review>
                {
                    Success = false,
                    Message = "Unauthorized User",
                    Data = null
                };
                }

            review.Comment = dto.Comment;
            review.rating = dto.rating;

            await _unitOfWork._reviewService.UpdateAsync(review);
            _unitOfWork.Complete();

            return new ApiResponse<Review>
            {
                Success = true ,
                Message = "Review Updated Successfully",
                Data = review
            };
        }
    }
}
