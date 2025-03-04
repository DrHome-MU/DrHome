using Dr_Home.Data.Models;

namespace Dr_Home.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAllAsync();

        Task<Patient> GetById(Guid id);

        Task AddAsync(Patient entity);

        Task<Patient> UpdateAsync(Patient entity);
        Task<Patient> DeleteAsync(Patient entity);
    }
}
