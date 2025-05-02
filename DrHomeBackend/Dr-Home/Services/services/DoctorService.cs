using Dr_Home.Data;
using Dr_Home.Data.Models;
using Dr_Home.DTOs.AppointmentDTOs;
using Dr_Home.DTOs.DoctorDtos;
using Dr_Home.Services.Interfaces;
using Mapster;
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

        public async Task<IEnumerable<GetDoctorDtoV2>> FilterDoctorAsync(DoctorFilterDto filter, CancellationToken cancellationToken = default)
        {
            var doctors = await db.Set<Doctor>().Include(d => d._specialization).Where(x =>
                (string.IsNullOrEmpty(filter.FullName) || filter.FullName ==  x.FullName) && 
                (string.IsNullOrEmpty(filter.city) || filter.city == x.city) && 
                (string.IsNullOrEmpty(filter.region) || filter.region == x.region) && 
                (filter.SpecializationId == 0 || (filter.SpecializationId == x.SpecializationId)))
                .ToListAsync(cancellationToken);

            var result = new List<GetDoctorDtoV2>();

            foreach(var doctor in doctors)
            {
                var item = doctor.Adapt<GetDoctorDtoV2>(); 
                item.specialization = doctor._specialization.Name;
                result.Add(item);   
            }

            return result;
               
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await db.Set<Doctor>().ToListAsync();
        }

        public async Task<Doctor> GetById(Guid id)
        {
            return await db.Set<Doctor>().Include(d => d._specialization).Include(d => d._appointments).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Doctor> UpdateAsync(Doctor entity)
        {
            db.Update(entity);
            return entity;
        }

    }
}
