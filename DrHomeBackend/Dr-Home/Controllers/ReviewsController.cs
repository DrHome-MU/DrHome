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
    public class ReviewsController(IReviewHelper _reviewHelper) : ControllerBase
    {
        /// <summary>
        /// Add Review By Patient
        /// </summary>
        /// <param name="dto">
        /// <br/>
        /// --<b>PatientId</b> The Id Of The Patient Who Will Review Specific Doctor<br/>
        /// --<b>DoctorId</b> The Id Of The Doctor who Will Be Reviewed<br/>
        /// --<b>Comment</b> Can be null
        /// --<b>rating</b>between 1 and 5
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code = "200" >Added Successfully</response>
        /// <response code = "404">Doctor Or Patient Is Not Found</response>
        /// <response code = "409">The Patient already added review to this doctor before</response>
        [HttpPost("")]
        [Authorize(Roles = "Patient")]
        [ProducesResponseType(typeof(GetReviewDto), 200)]
        public async Task<IActionResult> AddReview(AddReviewDto dto , CancellationToken cancellationToken)
        {
            var result = await _reviewHelper.AddReview(dto);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        /// <summary>
        /// Get Doctor Reviewes
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code = "200">Data Loaded Successfully(Can Be Empty)</response>
        /// <response code = "404">Doctor Doesn`t Exist</response>

        [HttpGet("{DoctorId}")]
        [ProducesResponseType(typeof(IEnumerable<GetReviewDto>) , 200)]
        public async Task<IActionResult>GetDoctorReviews([FromRoute] Guid DoctorId , CancellationToken cancellationToken)
        {
            var result = await _reviewHelper.GetDoctorReviews(DoctorId);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        /// <summary>
        /// Get The Reviews Of Specific Patient
        /// </summary>
        /// <param name="PatientId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("PatientReviews/{PatientId}")]
        [Authorize(Roles = "Admin,Patient")]
        [ProducesResponseType(typeof(IEnumerable<GetReviewDto>), 200)]
        public async Task<IActionResult> GetPatientReviews([FromRoute] Guid PatientId, CancellationToken cancellationToken)
        {
            var result = await _reviewHelper.GetPatientReviews(PatientId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


       

        /// <summary>
        /// Get The Average Rating of doctor
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code = "200">it will return bool to know which the doctor has reviews or not and the average rating</response>
        /// <response code = "404">Doctor doesn`t exist</response>

        [HttpGet("AverageRating")]
        [ProducesResponseType(typeof(GetAverageReviewDto), 200)]
        public async Task<IActionResult> GetAverageRating(Guid DoctorId , CancellationToken cancellationToken )
        {
            var result = await _reviewHelper.GetDoctorAverageRating(DoctorId , cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("GetReportedReviews")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetReportedReviews()
        {
            var result = await _reviewHelper.GetReportedReviews();

            return Ok(result.Value);
        }

        /// <summary>
        /// Update Review By Patient
        /// </summary>
        /// <param name="ReviewId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code = "204">Updated Successfully</response>
        /// <response code = "401">Unauthorized Update</response>
        /// <response code = "404">Review Doesn`t Exist</response>
        [HttpPut("{ReviewId}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdateReview([FromRoute] Guid ReviewId ,  UpdateReviewDto dto , CancellationToken cancellationToken)
        {
           var result = await _reviewHelper.UpdateReview(ReviewId,dto, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
        /// <summary>
        /// Report Review By The Doctor
        /// </summary>
        /// <param name="ReviewId"></param>
        /// <returns></returns>

        [HttpPut("ReportReview/{ReviewId}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> ReportReview([FromRoute] Guid ReviewId)
        {
            var result = await _reviewHelper.ReportReview(ReviewId);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
       /// <summary>
       /// Delete Review By Patient or admin
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       /// <response code = "204">Deleted Successfully</response>
       /// <response code = "404">Review Doesn`t Exist</response>

        [HttpDelete("{id}")]
        [Authorize(Roles = ("Admin,Patient"))]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
           var result = await _reviewHelper.DeleteReview(id);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
