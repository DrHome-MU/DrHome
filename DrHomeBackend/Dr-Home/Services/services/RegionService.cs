


namespace Dr_Home.Services.services
{
    public class RegionService(AppDbContext db) : IRegionService
    {
        public async Task<Region> AddAsync(Region region, CancellationToken cancellationToken = default)
        {
             await db.Set<Region>().AddAsync(region , cancellationToken);
            return region;
        }

       

        public async Task<bool> IsRegionExistedAsync(AddRegionDto dto, CancellationToken cancellationToken = default)
        {
            return await db.Set<Region>().AnyAsync(r => r.CityId == dto.CityId && r.Name == dto.name , cancellationToken);
        }

        public async Task<IEnumerable<Region>> GetCityRegionsAsync(int CityId, CancellationToken cancellationToken)
        {
            var values = await db.Set<Region>().Where(r => r.CityId == CityId).ToListAsync(cancellationToken);

            return values;
        }

        public async Task<IEnumerable<Region>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var regions = await db.Set<Region>().ToListAsync(cancellationToken);

            return regions;
        }
    }
}
