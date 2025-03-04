using Dr_Home.DTOs.DoctorDtos;
using Dr_Home.Helpers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dr_Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController(IDoctorHelper _doctorHelper) : ControllerBase
    {
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

            return Ok(new { Success = false, Message = response  , doctorId = response.Data.Id});
        }
        [HttpPut("UpdateData")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateDoctorData(UpdateDoctorDto dto)
        {
            if (!ModelState.IsValid) { return BadRequest("Unvalid Data"); }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(userId == null)return Unauthorized();

            Guid id = Guid.Parse(userId);

            var response = await _doctorHelper.UpdateDoctor(id, dto);


            return (!response.Success) ? 
                BadRequest(new { Success = false, Message = response.Message }):
                Ok(new { Success = true , Message = response.Message , 
                    doctorId = response.Data.Id });
                
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>DeleteDoctor(Guid doctorId)
        {
            var doctor = await _doctorHelper.DeleteDoctor(doctorId);

            if (doctor == null) { return BadRequest("There is no doctor with this id"); }

            return Ok(doctor);
        }
    }
}
