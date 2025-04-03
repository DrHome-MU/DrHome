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
        //Add Review By Patient
        [HttpPost("")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> AddReview(AddReviewDto dto)
        {
            var response = await _reviewHelper.AddReview(dto);

            return (!response.Success) ? BadRequest(response) : Ok(response);
        }

        //Get All Reviews 
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _reviewHelper.GetAll();

            return (response.Success) ? Ok(response) : NotFound(response);
        }

        //Get Reviews For Specific Doctor

        [HttpGet("{DoctorId}")]
        
        public async Task<IActionResult>GetReviews([FromRoute] Guid DoctorId)
        {
            var response = await _reviewHelper.GetDoctorReviews(DoctorId);

            return (!response.Success) ? NotFound(response) : Ok(response);
        }

        //Get Reviews Done by Specific Patient

        [HttpGet("")]
        [Authorize(Roles = "Patient")]

        public async Task<IActionResult>GetPatientReviews(Guid PatientId)
        {
            var response = await _reviewHelper.GetPatientReviews(PatientId);

            return (!response.Success) ? NotFound(response) : Ok(response);
        }

       

        //Get Average Rating Of  Doctor 

        [HttpGet("AverageRating")]
        public async Task<IActionResult> GetAverageRating(Guid DoctorId)
        {
            var response = await _reviewHelper.GetDoctorAverageRating(DoctorId);

            return (!response.Success) ? NotFound(response) : Ok(response);
        }

        //Update Review By Patient
        [HttpPut("{ReviewId}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdateReview([FromRoute] Guid ReviewId ,  UpdateReviewDto dto)
        {
            var response = await _reviewHelper.UpdateReview(ReviewId, dto);

            if(response.Message == "Unauthorized User")return Unauthorized(response);

            return (response.Success)? Ok(response) : NotFound(response);
        }
        //Delete Review By Patient Or Admin

        [HttpDelete("{id}")]
        [Authorize(Roles = ("Admin,Patient"))]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            var response = await _reviewHelper.DeleteReview(id);

            return (!response.Success) ? BadRequest(response) : Ok(response);
        }
    }
}
