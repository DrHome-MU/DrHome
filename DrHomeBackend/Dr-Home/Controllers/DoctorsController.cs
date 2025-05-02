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
    public class DoctorsController(IDoctorHelper _doctorHelper) : ControllerBase
    {
        /// Add Doctor
        [HttpPost("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>AddDoctor(AddDoctorDto dto)
        {

            var response = await _doctorHelper.AddDoctor(dto);

            if (response.Success == false)
                return BadRequest(new { Success = false, Message = response });

            return Ok(new { Success = true, Message = response  , doctorId = response.Data.Id});
        }

        ///Get All Doctors 

        [HttpGet("")]
        [Authorize]

        public async Task<IActionResult> GetAllDoctors()
        {
            var response = await _doctorHelper.GetDoctors();
            return (response.Success == true) ? Ok(response) : NotFound(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> ShowDoctorData([FromRoute]Guid id)
        {
            var response = await _doctorHelper.ShowDoctorData(id);

            return (response.Success == true) ? Ok(response) : NotFound(response);


        }
        /// <summary>
        /// Filter Doctor By FullName,city,region,specializationId
        /// </summary>
        /// <param name="filter">
        /// <br/>
        /// <para>كل القيم ممكن تتبعت او تتساب فاضية </para><br/>
        /// <b>FullName</b>: Must be at most 100 character
        /// 
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code = "200" >ممكن يرجعلك فيمة او ممكن تبقى قيمة فاضية</response>

        [HttpPost("FilterDoctors")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<GetDoctorDto>), 200)]
        public async Task<IActionResult> FilterDoctors([FromBody]DoctorFilterDto filter , CancellationToken cancellationToken)
        {
            var result = await _doctorHelper.FilterDoctors(filter , cancellationToken); 

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        /// Update Doctor Data
        [HttpPut("")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateDoctorData([FromForm] UpdateDoctorDto dto, CancellationToken cancellationToken)
        {

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
        [HttpPut("updatePicture")]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> UpdateDoctorProfilePicture([FromForm] UpdatePictureDto dto, CancellationToken cancellationToken) { 
            var result = await _doctorHelper.UpdateDoctorProfilePicture(dto, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }



        /// Delete Specific Doctor

        [HttpDelete("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctor(Guid doctorId)
        {
            var response = await _doctorHelper.DeleteDoctor(doctorId);


            return (response.Success == false) ? NotFound(response) : Ok(response);
        }
    }
}
