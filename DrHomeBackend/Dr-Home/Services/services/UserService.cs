using Dr_Home.Data;
using Dr_Home.Data.Models;
using Dr_Home.DTOs.AuthDTOs;
using Dr_Home.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dr_Home.Services.services
{
    public class UserService(AppDbContext db) : IUserService
    {
        public async Task AddAsync(User entity)
        {
            await db.Set<User>().AddAsync(entity);
        }

        public async Task<User> DeleteAsync(User entity)
        {
            db.Remove(entity);
            return entity;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await db.Set<User>().Where(x => x.role == "Patient").ToListAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await db.Set<User>().FirstOrDefaultAsync(x=>x.Email == email);
        }

        public async Task<User> GetById(Guid id)
        {
            return await db.Set<User>().FindAsync(id);
        }

        

        public async Task<bool> IsEmailExists(string email)
        {
           return  await db.Set<User>().AnyAsync(u => u.Email == email);
        }

        public async Task<User> UpdateAsync(User entity)
        {
            db.Update(entity);
            return entity;
        }
    }
}
