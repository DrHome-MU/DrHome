using Dr_Home.Helpers.helpers;

namespace Dr_Home.Helpers.Interfaces
{
    public interface ICityHelper
    {
        public Task<ApiResponse<IEnumerable<City>>> GetAllAsync(string lang); 
    }
}
