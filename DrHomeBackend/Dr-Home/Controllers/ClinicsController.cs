using Dr_Home.DTOs.ClinicDtos;
using Dr_Home.Helpers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dr_Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicsController(IClinicHelper _clinicHelper) : ControllerBase
    {
        /// Add Clinic 
        [HttpPost("Add")]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> AddClinic(AddClinicDto dto)
        {
            if (!ModelState.IsValid) { return BadRequest(new { Success = false, Message = ModelState }); }

            var response = await _clinicHelper.AddClinic(dto);

            return (response.Success == true) ? Ok(new { response.Success, response.Message }) : NotFound(response);
        }

        /// Get All Clinics
        [HttpGet("")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAllClinics()
        {
            var response = await _clinicHelper.GetAllClincs();

            return (response.Success == true) ? Ok(response) : NotFound(response);
        }
        /// Get Doctor`s Clinics
        [HttpGet("DoctorClinics")]
        [Authorize]

        public async Task<IActionResult> GetDoctorClinics(Guid DoctorId)
        {
            var response = await _clinicHelper.GetDoctorClinics(DoctorId);

            return (response.Success == true) ? Ok(response) : NotFound(response);
        }

        ///Update Clinic 

        [HttpPut("Update")]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> UpdateClinic(UpdateClinicDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ModelState
                });
            }
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (doctorId == null) { return Unauthorized(new { Success = false, message = "Unauthorized User" }); }

            var response = await _clinicHelper.UpdateDoctorClinic(Guid.Parse(doctorId), dto);

            if (response.Message == "Unauthorized") return Unauthorized(response);

            return (response.Success == true) ? Ok(response) : NotFound(response);

        }

        /// Delete Clinic

        [HttpDelete("DeleteClinic")]
        [Authorize(Roles = "Admin , Doctor")]

        public async Task<IActionResult> DeleteClinic(Guid ClinicId)
        {
            if (!ModelState.IsValid) { return BadRequest(new { Success = false, Message = ModelState }); }

            var response = await _clinicHelper.DeleteClinic(ClinicId);

            return (response.Success == true) ? Ok(response) : NotFound(response);

        }
        
    }
}
