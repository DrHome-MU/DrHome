namespace Dr_Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController(IRegionHelper _regionHelper) : ControllerBase
    {

        [HttpGet("{CityId}")]

        public async Task<IActionResult> GetCityRegions([FromRoute] int CityId,  string lang = "ar" )
        {
            var response = await _regionHelper.GetCityRegionsAsync(CityId, lang);
            return (response.Success ? Ok(response) : NotFound(response));
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] string lang = "ar")
        {
            var response = await _regionHelper.GetAllRegionsAsync(lang);

            return (response.Success ? Ok(response) : NotFound(response));
        }

    }
}
