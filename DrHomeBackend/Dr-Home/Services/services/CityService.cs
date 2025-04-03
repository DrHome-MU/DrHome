


namespace Dr_Home.Services.services
{
    public class CityService(AppDbContext db) : ICityService
    {
        public async Task<City> AddAsync(City city, CancellationToken cancellationToken = default)
        {
            await db.Set<City>().AddAsync(city , cancellationToken);

            return city;
        }

        public async Task<IEnumerable<City>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var cities = await db.Set<City>().ToListAsync(cancellationToken);   
            return cities;
        }

        public async Task<City> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
           var city = await db.Set<City>().FirstOrDefaultAsync(x => x.Name == name , cancellationToken);
            return city;
        }
    }
}
