using Dr_Home.Data;
using Dr_Home.Data.Models;
using Dr_Home.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dr_Home.Services.services
{
    public class DoctorService(AppDbContext db) : IDoctorService
    {
        public async Task AddAsync(Doctor entity)
        {
            await db.Set<Doctor>().AddAsync(entity);
        }

        public async Task<Doctor> DeleteAsync(Doctor entity)
        {
            db.Remove(entity);
            return entity;
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await db.Set<Doctor>().ToListAsync();
        }

        public async Task<Doctor> GetById(Guid id)
        {
            return await db.Set<Doctor>().FindAsync(id);
        }
        public async Task<Doctor> UpdateAsync(Doctor entity)
        {
            db.Update(entity);
            return entity;
        }

    }
}
