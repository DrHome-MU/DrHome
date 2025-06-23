using Dr_Home.Data.Models;
using Dr_Home.Helpers.helpers;

namespace Dr_Home.Helpers.Interfaces
{
    public interface ISpecializationHelper
    {
        Task<ApiResponse<Specialization>> AddAsync(string name , CancellationToken cancellationToken = default); 

        Task<ApiResponse<IEnumerable<Specialization>>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<ApiResponse<Specialization>> DeleteAsync(int id , CancellationToken cancellationToken = default);

        Task<Result> updateAsync(int id, IFormFile? _pic, CancellationToken cancellationToken = default);
    }
}
