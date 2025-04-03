namespace Dr_Home.Services.Interfaces
{
    public interface IRegionService
    {
        public Task<Region> AddAsync(Region region , CancellationToken cancellationToken = default);

        public Task<bool> IsRegionExistedAsync(AddRegionDto dto, CancellationToken cancellationToken = default);

        public Task<IEnumerable<Region>> GetCityRegionsAsync(int CityId , CancellationToken cancellationToken = default);

        public Task<IEnumerable<Region>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
