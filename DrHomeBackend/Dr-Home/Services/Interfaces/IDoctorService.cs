using Dr_Home.Data.Models;
using Dr_Home.DTOs.DoctorDtos;

namespace Dr_Home.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<Doctor>> GetAllAsync();

        Task<Doctor> GetById(Guid id);

        Task AddAsync(Doctor entity);

        Task<Doctor> UpdateAsync(Doctor entity);
        Task<Doctor> DeleteAsync(Doctor entity);

        Task<IEnumerable<GetDoctorDto>> FilterDoctorAsync(DoctorFilterDto filter , CancellationToken cancellationToken = default);
    }
}
