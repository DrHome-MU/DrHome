namespace Dr_Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController(IRegionHelper _regionHelper) : ControllerBase
    {

        [HttpPost("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRegion([FromBody] AddRegionDto dto, CancellationToken cancellationToken)
        {
            var response = await _regionHelper.AddRegionAsync(dto, cancellationToken);
            return (response.Success ? Ok(response) : BadRequest(response));
        }

        [HttpGet("{CityId}")]

        public async Task<IActionResult> GetCityRegions([FromRoute] int CityId, CancellationToken cancellationToken)
        {
            var response = await _regionHelper.GetCityRegionsAsync(CityId, cancellationToken);
            return (response.Success ? Ok(response) : NotFound(response));
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var response = await _regionHelper.GetAllRegionsAsync(cancellationToken);

            return (response.Success ? Ok(response) : NotFound(response));
        }

    }
}
