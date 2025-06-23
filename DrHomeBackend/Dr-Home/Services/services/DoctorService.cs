using Dr_Home.Data;
using Dr_Home.Data.Models;
using Dr_Home.DTOs.AppointmentDTOs;
using Dr_Home.DTOs.DoctorDtos;
using Dr_Home.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Dr_Home.Services.services
{
    public class DoctorService(AppDbContext db, IServiceProvider serviceProvider) : IDoctorService
    {
        
        private readonly IServiceProvider _serviceProvider = serviceProvider;

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
            var data = await db.Set<Clinic>().Where(clinic =>
                (string.IsNullOrEmpty(filter.FullName) || filter.FullName == clinic.doctor!.FullName) &&
                (string.IsNullOrEmpty(filter.city) || filter.city == clinic.city) &&
                (string.IsNullOrEmpty(filter.region) || filter.region == clinic.region) &&
                (filter.SpecializationId == 0 || (filter.SpecializationId == clinic.doctor!.SpecializationId)))
                .Select(clinic => new GetDoctorDtoV2()
                {
                    doctorId = clinic.doctor!.Id,
                    doctorName = clinic.doctor.FullName,

                    specialization = clinic.doctor._specialization.Name,

                    profilePicPath = clinic.doctor.ProfilePic_Path,

                    doctorSummary = clinic.doctor.Summary,

                    clinicId = clinic.Id,

                    clinicName = clinic.ClinicName,

                    clinicPhone = clinic.PhoneNumber,

                    appointmentFee = clinic.AppointmentFee,

                    clinicCity = clinic.city,

                    clinicRegion = clinic.region,

                    detailedAddress = clinic.DetailedAddress,
                }).ToListAsync(cancellationToken);

            //var _scheduleHelper = _serviceProvider.GetRequiredService<IScheduleHelper>();
            //var _reviewHelper = _serviceProvider.GetRequiredService<IReviewHelper>();

            //foreach (var item in data)
            //{
            //    var reviwesResult = await _reviewHelper.GetDoctorReviews(item.doctorId);
            //    item.doctorReviews = reviwesResult.Value;
            //    var schedulesResult = await _scheduleHelper.GetSchedulesAsync(item.doctorId, item.clinicId);
            //    item.schedules = schedulesResult.Value;
            //}

            return data;

        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await db.Set<Doctor>().Include(d => d._specialization).ToListAsync();
        }

        public async Task<Doctor> GetById(Guid id)
        {
            return await db.Set<Doctor>().Include(d => d._specialization).Include(d => d._appointments).Include(d=>d.clinics).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Doctor> UpdateAsync(Doctor entity)
        {
            db.Update(entity);
            return entity;
        }

    }
}
