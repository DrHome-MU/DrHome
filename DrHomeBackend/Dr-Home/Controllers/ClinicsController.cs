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
        [HttpPost("")]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> AddClinic(AddClinicDto dto)
        {
            var response = await _clinicHelper.AddClinic(dto);

            return (response.Success == true) ? Ok(response) : BadRequest(response);
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
        [HttpGet("{DoctorId}")]
        [Authorize]

        public async Task<IActionResult> GetDoctorClinics([FromRoute] Guid DoctorId)
        {
            var response = await _clinicHelper.GetDoctorClinics(DoctorId);

            return (response.Success == true) ? Ok(response) : NotFound(response);
        }

        ///Update Clinic 

        [HttpPut("{ClinicId}")]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> UpdateClinic([FromRoute] Guid ClinicId , UpdateClinicDto dto)
        {
            var response = await _clinicHelper.UpdateDoctorClinic(ClinicId, dto);

            if (response.Message == "Unauthorized") return Unauthorized(response);

            return (response.Success == true) ? Ok(response) : NotFound(response);

        }

        /// Delete Clinic

        [HttpDelete("{ClinicId}")]
        [Authorize(Roles = "Admin , Doctor")]

        public async Task<IActionResult> DeleteClinic([FromRoute]Guid ClinicId)
        {
            if (!ModelState.IsValid) { return BadRequest(new { Success = false, Message = ModelState }); }

            var response = await _clinicHelper.DeleteClinic(ClinicId);

            return (response.Success == true) ? Ok(response) : NotFound(response);

        }
        
    }
}
