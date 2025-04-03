using Dr_Home.Data;
using Dr_Home.Data.Models;
using Dr_Home.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dr_Home.Services.services
{
    public class SpecializationService(AppDbContext db) : ISpecializationService
    {
        public async Task<Specialization> Add(Specialization entity, CancellationToken cancellationToken)
        {
            await db.Set<Specialization>().AddAsync(entity,cancellationToken);
            return entity;
        }

        

        public async Task<IEnumerable<Specialization>> GetAll(CancellationToken cancellationToken)
        {
            var entites = await db.Set<Specialization>().ToListAsync(cancellationToken);

            return entites;
        }

        public async Task<Specialization> GetByName(string Name, CancellationToken cancellationToken)
        {
            var entity = await db.Set<Specialization>().FirstOrDefaultAsync(x => x.Name == Name, cancellationToken);
            return entity;
        }
        public async Task<Specialization> DeleteAsync(Specialization entity, CancellationToken cancellationToken = default)
        {
            db.Remove(entity);
            return entity;

        }

        public async Task<Specialization> GetByIdAsync(int id, CancellationToken cancellationToken  = default)
        {
            var entity = await db.Set<Specialization>().FindAsync(id, cancellationToken);

            return entity;
        }
    }
}
