using Dr_Home.Data;
using Dr_Home.Data.Models;
using Dr_Home.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dr_Home.Services.services
{
    public class ClinicService(AppDbContext db) : IClinicService
    {
        public async Task<Clinic> AddClinicAsync(Clinic clinic)
        {
           await db.Set<Clinic>().AddAsync(clinic);
            return clinic;
        }

        public async Task<Clinic> DeleteClinicAsync(Clinic clinic)
        {
            db.Remove(clinic);
            return clinic;
        }

        public async Task<IEnumerable<Clinic>> GetAllClinicAsync()
        {
            var clinics = await db.Set<Clinic>().ToListAsync();

            return clinics;
        }

        public async Task<Clinic> GetById(Guid id)
        {
            return await db.Set<Clinic>().FindAsync(id);
        }

        public async Task<Clinic> GetClinicByNameAndCityAndRegion(string name, string city, string region)
        {
            var clinic  = await db.Set<Clinic>().FirstOrDefaultAsync(x=>x.ClinicName == name 
            && x.city == city && x.region == region);

            return clinic; 
        }

        public async Task<IEnumerable<Clinic>> GetDoctorClinicsAsync(Guid DoctorId)
        {
            var clinics = await db.Set<Clinic>().Where(c => c.DoctorId == DoctorId).ToListAsync();

            return clinics;
        }

        public async Task<Clinic> UpdateClinic(Clinic clinic)
        {
            db.Update(clinic);
            return clinic;
        }
    }
}
