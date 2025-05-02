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
        public async Task<Result<GetReviewDto>> AddReview(AddReviewDto dto , CancellationToken cancellationToken = default)
        {

            var doctor = await _unitOfWork._doctorService.GetById(dto.DoctorId);
            var patient = await _unitOfWork._patientService.GetById(dto.PatientId);

            if (doctor == null)
                return Result.Failure<GetReviewDto>(DoctorErrors.DoctorNotFound);

            if(patient == null)
                return Result.Failure<GetReviewDto>(PatientErrors.PatientNotFound);

            var isPatientGiveReviewBefore = await _unitOfWork._reviewService.IsPatientReviewedBefore(dto.PatientId,dto.DoctorId);

            if(isPatientGiveReviewBefore)
                return Result.Failure<GetReviewDto>(ReviewErrors.PatientMadeReviewBefore);

            var review = dto.Adapt<Review>();
            review.ReviewTime = DateTime.Now;

            await _unitOfWork._reviewService.AddAsync(review);
            await _unitOfWork.Complete();


            var result = review.Adapt<GetReviewDto>();
            result.ReviwerName = patient.FullName;

            return Result.Success<GetReviewDto>(result);

        }

        public async Task<Result> DeleteReview(Guid id , CancellationToken cancellationToken = default)
        {
            var review = await _unitOfWork._reviewService.GetReviewById(id);

            if (review == null)
            return Result.Failure(ReviewErrors.ReviewNotFound);

            await _unitOfWork._reviewService.DeleteAsync(review);
            await _unitOfWork.Complete(cancellationToken);

            return Result.Success();
        }



        public async Task<Result<GetAverageReviewDto>> GetDoctorAverageRating(Guid DoctorId , CancellationToken cancellationToken = default)
        {
            var doctor = await _unitOfWork._doctorService.GetById(DoctorId);

            if (doctor == null)
                return Result.Failure<GetAverageReviewDto>(DoctorErrors.DoctorNotFound);

            var reviews = await _unitOfWork._reviewService.GetDoctorReviews(DoctorId);

            
            var sumRating = reviews.Sum(r => r.rating);

            int reviewsCount = reviews.Count();

            if (reviewsCount == 0) 
                return Result.Success(new GetAverageReviewDto { DoctorHasReviewes = false , rating = (decimal)reviewsCount}); 

            decimal averageRating = (decimal)sumRating / reviewsCount;

            return Result.Success(new GetAverageReviewDto { DoctorHasReviewes = true , rating = averageRating});

        }

        public async Task<Result<IEnumerable<GetReviewDto>>> GetDoctorReviews(Guid DoctorId , CancellationToken cancellationToken = default)
        {
            var doctor = await _unitOfWork._doctorService.GetById(DoctorId); 

            if(doctor == null)
                return Result.Failure<IEnumerable<GetReviewDto>>(DoctorErrors.DoctorNotFound);

            var reviews = await _unitOfWork._reviewService.GetDoctorReviews(DoctorId);
 

            List<GetReviewDto> result = new List<GetReviewDto>();

            foreach(var review in reviews)
            {
                var dto = review.Adapt<GetReviewDto>();
                dto.ReviwerName = review.patient!.FullName;
                result.Add(dto);
            }

            return Result.Success<IEnumerable<GetReviewDto>>(result);

        }

        public async  Task<Result> UpdateReview(Guid ReviewId, UpdateReviewDto dto , CancellationToken cancellationToken = default)
        {
            var review = await _unitOfWork._reviewService.GetReviewById(ReviewId);

            if (review == null)
            return Result.Failure(ReviewErrors.ReviewNotFound);

            if(review.patient!.Id != dto.PatientId) 
                return Result.Failure(ReviewErrors.UnauthorizedUpdateReview);

            review.Comment = dto.Comment;
            review.rating = dto.rating;

            await _unitOfWork._reviewService.UpdateAsync(review);
            await _unitOfWork.Complete(cancellationToken);

             return Result.Success();
            }
        }
    }

