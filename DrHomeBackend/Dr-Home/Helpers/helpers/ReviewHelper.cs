using Dr_Home.Data.Models;
using Dr_Home.DTOs.ReviewDtos;
using Dr_Home.Helpers.Interfaces;
using Dr_Home.Services.Interfaces;
using Dr_Home.UnitOfWork;
using Mapster;

namespace Dr_Home.Helpers.helpers
{
    public class ReviewHelper(IUnitOfWork _unitOfWork) : IReviewHelper
    {
        public async Task<ApiResponse<GetReviewDto>> AddReview(AddReviewDto dto)
        {

            var review = dto.Adapt<Review>();
            review.ReviewTime = DateTime.Now;

            var doctor = await _unitOfWork._doctorService.GetById(dto.DoctorId);

            if (doctor == null)
                return new ApiResponse<GetReviewDto>
                {
                    Success = false,
                    Message = "Doctor Not Found"
                };

            await _unitOfWork._reviewService.AddAsync(review);
            await _unitOfWork.Complete();

            var res = await _unitOfWork._reviewService.GetReviewById(review.Id);

            var result = res.Adapt<GetReviewDto>();

            return new ApiResponse<GetReviewDto>
            {
                Success = true,
                Message = "Review Is Added Successfully",
                Data = result
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

            await _unitOfWork._reviewService.DeleteAsync(review);
            await _unitOfWork.Complete();

            return new ApiResponse<Review>
            {
                Success = true,
                Message = "Deleted Successfully."
            };
        }

        public async Task<ApiResponse<IEnumerable<GetReviewDto>>> GetAll()
        {
           var reviews = await _unitOfWork._reviewService.GetAll();

           var result = reviews.Adapt<IEnumerable<GetReviewDto>>();

            if (!result.Any())
                return new ApiResponse<IEnumerable<GetReviewDto>>
                {
                    Success = false,
                    Message = "There Is No Reviews",
                    Data = result
                };

            return new ApiResponse<IEnumerable<GetReviewDto>>
            {
                Success = true,
                Message = "Reviews Loaded Successfully",
                Data = result
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
 
           
            if(reviews == null || reviews.Count() == 0)
            {
                return new ApiResponse<IEnumerable<GetReviewDto>>
                {
                    Success = false,
                    Message = "There Is No Reviews For this Doctor"
                };
            }

            List<GetReviewDto> result = new List<GetReviewDto>();

            foreach(var review in reviews)
            {
                var dto = review.Adapt<GetReviewDto>();
                dto.ReviwerName = review.patient!.FullName;
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
                    Message = "This User didn`t Review any doctor"
                };
            }

            List<GetReviewDto> result = new List<GetReviewDto>();

            foreach (var review in reviews)
            {
                var dto = review.Adapt<GetReviewDto>();
                dto.ReviwerName = review.patient!.FullName;
                result.Add(dto);
            }

            return new ApiResponse<IEnumerable<GetReviewDto>>
            {
                Success = true,
                Message = "Loading Reviews Is Done Successfully",
                Data = result
            };
        }

        public async  Task<ApiResponse<GetReviewDto>> UpdateReview(Guid ReviewId, UpdateReviewDto dto)
        {
            var review = await _unitOfWork._reviewService.GetReviewById(ReviewId);

            if (review == null)
            {
                return new ApiResponse<GetReviewDto>
                {
                    Success = false,
                    Message = "Review Doesn`t Exist"
                };
            }

            if(review.patient!.Id != dto.PatientId) {
                return new ApiResponse<GetReviewDto>
                {
                    Success = false,
                    Message = "Unauthorized User"
                };
                }

            review.Comment = dto.Comment;
            review.rating = dto.rating;

            await _unitOfWork._reviewService.UpdateAsync(review);
            await _unitOfWork.Complete();

            var result = review.Adapt<GetReviewDto>();

            result.ReviwerName = review.patient.FullName;

            return new ApiResponse<GetReviewDto>
            {
                Success = true ,
                Message = "Review Updated Successfully",
                Data = result
            };
        }
    }
}
