using Dr_Home.Helpers.helpers;

namespace Dr_Home.Helpers.Interfaces
{
    public interface ICityHelper
    {

        public Task<ApiResponse<City>> AddAsync(string name , CancellationToken cancellationToken = default);

        public Task<ApiResponse<IEnumerable<City>>> GetAllAsync(CancellationToken cancellationToken = default); 
    }
}
