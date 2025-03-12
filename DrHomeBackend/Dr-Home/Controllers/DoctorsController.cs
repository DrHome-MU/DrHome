using Dr_Home.DTOs.DoctorDtos;
using Dr_Home.Helpers.Interfaces;
using Dr_Home.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dr_Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController(IDoctorHelper _doctorHelper , IUnitOfWork unitOfWork) : ControllerBase
    {
        /// Add Doctor
        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>AddDoctor(AddDoctorDto dto)
        {
            if (!ModelState.IsValid ) 
            { return BadRequest(ModelState); }

            if(dto.ConfirmPassword != dto.Password)
            { return BadRequest("Make sure that the confirmed password is equal to the password"); }

            var response = await _doctorHelper.AddDoctor(dto);

            if (response.Success == false)
                return BadRequest(new { Success = false, Message = response });

            return Ok(new { Success = true, Message = response  , doctorId = response.Data.Id});
        }

        ///Get All Doctors 

        [HttpGet("")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAllDoctors()
        {
            var response = await _doctorHelper.GetDoctors();
            return (response.Success == true) ? Ok(response) : NotFound(response);
        }


        /// Test Image
        [HttpGet("DoctorImage")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetImage()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(doctorId == null) { return BadRequest(); }

            var doctor = await unitOfWork._doctorService.GetById(Guid.Parse(doctorId));

            if (doctor == null) return NotFound();

            var request = HttpContext.Request;

            return Ok(new
            {
                Success = true,
                img = doctor.ProfilePic_Path
            });



        }
        /// Update Doctor Data
        [HttpPut("UpdateData")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateDoctorData([FromForm] UpdateDoctorDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Unvalid Data!"
                });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return Unauthorized(new
            {
                success = false,
                message = "Unauthorized User!"
            });

            Guid id = Guid.Parse(userId);

            var response = await _doctorHelper.UpdateDoctor(id, dto, cancellationToken);


            return (!response.Success) ?
                BadRequest(new { Success = false, Message = response.Message }) :
                Ok(new
                {
                    Success = true,
                    Message = response.Message,
                    doctorId = response.Data.Id
                });

        }



        /// Delete Specific Doctor

        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctor(Guid doctorId)
        {
            var response = await _doctorHelper.DeleteDoctor(doctorId);


            return (response.Success == false) ? NotFound(response) : Ok(response);
        }
    }
}
