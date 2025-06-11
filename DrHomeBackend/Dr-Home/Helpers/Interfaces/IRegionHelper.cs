

namespace Dr_Home.Helpers.Interfaces
{
    public interface IRegionHelper
    {

        public Task<ApiResponse<IEnumerable<Region>>> GetCityRegionsAsync(int CityId , string lang);

        public Task<ApiResponse<IEnumerable<Region>>>GetAllRegionsAsync(string lang);

    }
}
