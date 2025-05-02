using Dr_Home.Data;
using Dr_Home.Data.Models;
using Dr_Home.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dr_Home.Services.services
{
    public class PatientService(AppDbContext db) : IPatientService
    {
        public async Task AddAsync(Patient entity)
        {
            await db.Set<Patient>().AddAsync(entity);
        }

        public async Task<Patient> DeleteAsync(Patient entity)
        {
            db.Remove(entity);
            return entity;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await db.Set<Patient>().ToListAsync();
        }

        public async Task<Patient> GetById(Guid id)
        {
            return await db.Set<Patient>().Include(p => p.Reviews).Include(p => p._appointments).
                 FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Patient> UpdateAsync(Patient entity)
        {
            db.Update(entity);
            return entity;
        }
    }
}
