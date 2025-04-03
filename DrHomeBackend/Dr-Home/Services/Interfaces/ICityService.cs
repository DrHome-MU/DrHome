namespace Dr_Home.Services.Interfaces
{
    public interface ICityService
    {
        public Task<City> AddAsync(City city , CancellationToken cancellationToken = default);

        public Task<IEnumerable<City>> GetAllAsync(CancellationToken cancellationToken = default);

        public Task<City>GetByNameAsync(string name , CancellationToken cancellationToken = default);
    }
}
