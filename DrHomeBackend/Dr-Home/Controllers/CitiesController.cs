using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController(ICityHelper _cityHelper) : ControllerBase
    {

        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] string lang = "ar")
        {
            var response = await _cityHelper.GetAllAsync(lang);

            return (response.Success ? Ok(response) : BadRequest(Response));
        }

       
    }
}
