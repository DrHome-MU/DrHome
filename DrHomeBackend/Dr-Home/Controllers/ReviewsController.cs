using Dr_Home.Data.Models;
using Dr_Home.DTOs.ReviewDtos;
using Dr_Home.Helpers.Interfaces;
using Dr_Home.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Security.Claims;

namespace Dr_Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController(IReviewHelper _reviewHelper, IUnitOfWork unitOfWork) : ControllerBase
    {
        //Add Review By Patient
        [HttpPost("add")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> AddReview(AddReviewDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(new { Success = false, message = "Unauthorized User" });
            }

            var response = await _reviewHelper.AddReview(Guid.Parse(userId), dto);


            return (!response.Success) ? BadRequest(response) : Ok(response);
        }
        //Get Reviews For Specific Doctor

        [HttpGet("GetReviews")]
        
        public async Task<IActionResult>GetReviews(Guid DoctorId)
        {
            var response = await _reviewHelper.GetDoctorReviews(DoctorId);

            return (!response.Success) ? NotFound(response) : Ok(response);
        }

        //Get Reviews Done by Specific Patient

        [HttpGet("patientReviews")]

        public async Task<IActionResult>GetPatientReviews(Guid PatientId)
        {
            var response = await _reviewHelper.GetPatientReviews(PatientId);

            return (!response.Success) ? NotFound(response) : Ok(response);
        }

        //[HttpGet("")]
        //public async Task<IActionResult>Get(Guid id)
        //{
        //    var review = await unitOfWork._reviewService.GetReview(id);
        //    return Ok(new {patient = review.patient.FullName , doctor = review.doctor.FullName , review.Comment});
        //}

        //Get Average Rating Of  Doctor 

        [HttpGet("AverageRating")]
        public async Task<IActionResult> GetAverageRating(Guid DoctorId)
        {
            var response = await _reviewHelper.GetDoctorAverageRating(DoctorId);

            return (!response.Success) ? NotFound(response) : Ok(response);
        }

        //Update Review By Patient
        [HttpPut("update")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdateReview(UpdateReviewDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(new { Success = false, message = "Unauthorized User" });
            }

            var response = await _reviewHelper.UpdateReview(Guid.Parse(userId), dto);

            return (!response.Success) ? BadRequest(response) :

                Ok(new
                {
                    success = true,
                    message = "Review Updated Successfully",
                    Date = new
                    {
                        PatientName = response.Data.patient.FullName,
                        Comment = response.Data.Comment,
                        Date = response.Data.ReviewTime,
                        rating = response.Data.rating
                    },
                    updateDate = DateTime.UtcNow

                });
        }
        //Delete Review By Patient Or Admin

        [HttpDelete("delete")]
        [Authorize(Roles = ("Admin,Patient"))]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            var response = await _reviewHelper.DeleteReview(id);

            return (!response.Success) ? BadRequest(response) : Ok(response);
        }
    }
}
