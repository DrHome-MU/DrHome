

namespace Dr_Home.Helpers.Interfaces
{
    public interface IRegionHelper
    {
        public Task<ApiResponse<Region>> AddRegionAsync(AddRegionDto dto , CancellationToken cancellationToken = default);

        public Task<ApiResponse<IEnumerable<Region>>> GetCityRegionsAsync(int CityId , CancellationToken cancellationToken = default);

        public Task<ApiResponse<IEnumerable<Region>>>GetAllRegionsAsync(CancellationToken cancellationToken = default);

    }
}
