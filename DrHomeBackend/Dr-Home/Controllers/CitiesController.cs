using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController(ICityHelper _cityHelper) : ControllerBase
    {

        [HttpPost("")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Add(string name, CancellationToken cancellationToken)
        {
            var response = await _cityHelper.AddAsync(name, cancellationToken);

            return (response.Success) ? Ok(response) : BadRequest(response);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var response = await _cityHelper.GetAllAsync(cancellationToken);

            return (response.Success ? Ok(response) : BadRequest(Response));
        }

       
    }
}
