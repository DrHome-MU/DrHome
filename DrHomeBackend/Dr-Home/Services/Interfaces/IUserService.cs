using Dr_Home.Data.Models;
using Dr_Home.DTOs.AuthDTOs;

namespace Dr_Home.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> GetById(Guid id);

        Task AddAsync(User entity);

        Task<User> UpdateAsync(User entity);
        Task<User> DeleteAsync(User entity);
        Task<User> GetByEmail(string email);
        Task<bool>IsEmailExists(string email);
    }
}
