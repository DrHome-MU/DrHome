using Dr_Home.Data.Models;

namespace Dr_Home.Services.Interfaces
{
    public interface ISpecializationService
    {
        Task<Specialization> Add(Specialization entity, CancellationToken cancellationToken = default);

        Task<IEnumerable<Specialization>> GetAll(CancellationToken cancellationToken = default);

        Task<Specialization> GetByName(string Name, CancellationToken cancellationToken = default);

        Task<Specialization> DeleteAsync(Specialization entity, CancellationToken cancellationToken = default);

        Task<Specialization> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
