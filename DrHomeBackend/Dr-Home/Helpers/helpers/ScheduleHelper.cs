

using Azure;
using Dr_Home.Data.Models;
using Dr_Home.Errors;
using Mapster;
using System.Collections.Generic;

namespace Dr_Home.Helpers.helpers
{
    public class ScheduleHelper(IUnitOfWork unitOfWork , ILogger<ScheduleHelper>logger , AppDbContext db) : IScheduleHelper
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<ScheduleHelper> _logger = logger;
        private readonly AppDbContext _db = db;

        public async Task<Result<ScheduleResponse>> AddScheduleAsync(Guid DoctorId , Guid ClinicId, ScheduleRequest request, CancellationToken cancellationToken = default)
        {
           var clinic = await _unitOfWork._clinicalService.GetById(ClinicId);

            if (clinic == null)
                return Result.Failure<ScheduleResponse>(ClinicErrors.ClinicNotFound);

            if(clinic.DoctorId != DoctorId)
                return Result.Failure<ScheduleResponse>(ClinicErrors.ClinicNotFound);

            if(await _unitOfWork._scheduleService.SearchOnConflict(ClinicId , request))
                return Result.Failure<ScheduleResponse>(ScheduleErrors.ScheduleConflict);

            var schedule= request.Adapt<Doctor_Schedule>(); 

            schedule.ClinicId = ClinicId;

            await _unitOfWork._scheduleService.AddAsync(schedule,cancellationToken);

            await _unitOfWork.Complete(cancellationToken);

            var response = schedule.Adapt<ScheduleResponse>();

            var currentTime = request.StartTime;


            while (currentTime.AddMinutes(request.AppointmentDurationInMiniutes) <= request.EndTime)
            {
                response.ValidTimesForBooking[currentTime] = true;
                currentTime = currentTime.AddMinutes(request.AppointmentDurationInMiniutes);
            }

            return Result.Success<ScheduleResponse>(response);


        }

        public async Task<Result> DeleteAsync(Guid ScheduleId, CancellationToken cancellationToken = default)
        {
           var schedule = await _unitOfWork._scheduleService.GetByIdAsync(ScheduleId);

            if(schedule == null)
                return Result.Failure(ScheduleErrors.ScheduleNotFound);

            bool scheduleCanBeDeleted = ((schedule.WorkDay == DateOnly.FromDateTime(DateTime.UtcNow) 
                && schedule.EndTime >= TimeOnly.FromDateTime(DateTime.UtcNow)) || schedule._appointments!.Count == 0);

            if (!scheduleCanBeDeleted)
                return Result.Failure(ScheduleErrors.ScheduleCannotBeDeleted);

            var appointments = schedule._appointments;
            
            
            foreach(var item in appointments!)
            {
                item.IsActive = false;
                item.ScheduleId = null;
            }

            await _unitOfWork._scheduleService.DeleteAsync(schedule);
            await _unitOfWork.Complete(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<ScheduleResponse>> GetSchedueleAsync(Guid ClinicId, Guid ScheduleId, CancellationToken cancellationToken = default)
        {
            var schedule = await _db.Set<Doctor_Schedule>()
                .Include(s => s._appointments)
                .FirstOrDefaultAsync(s => s.Id == ScheduleId && s.ClinicId == ClinicId);

            if (schedule == null)
                return Result.Failure<ScheduleResponse>(ScheduleErrors.ScheduleNotFound);

            var response = schedule.Adapt<ScheduleResponse>();

            var currentTime = schedule.StartTime;

            while (currentTime.AddMinutes(schedule.AppointmentDurationInMiniutes) <= schedule.EndTime)
            {
                var IsBooked = (schedule._appointments!.Any(a => a.IsActive == true &&
                   a.AppointmentTime == currentTime));

                response.ValidTimesForBooking[currentTime] = !IsBooked;
                currentTime = currentTime.AddMinutes(schedule.AppointmentDurationInMiniutes);
            }

            return Result.Success(response);
        }

        public async Task<Result<IEnumerable<ScheduleResponse>>> GetSchedulesAsync(Guid DoctorId, Guid ClinicId, CancellationToken cancellationToken = default)
        {
            var clinic = await _db.Set<Clinic>().FirstOrDefaultAsync(c => c.Id == ClinicId && c.DoctorId == DoctorId);

            if (clinic == null)
            return Result.Failure<IEnumerable<ScheduleResponse>>(ClinicErrors.ClinicNotFound);

            var schedules = await _db.Set<Doctor_Schedule>()
                .Include(s => s._appointments)
                .Include(s => s.clinic)
                .Where(s => s.ClinicId == ClinicId && s.clinic!.DoctorId == DoctorId)
                .ToListAsync(cancellationToken);

           var result = new List<ScheduleResponse>();

            foreach (var scheduele in schedules)
            {
                var currentTime = scheduele.StartTime;
                var response = scheduele.Adapt<ScheduleResponse>();

                while (currentTime.AddMinutes(scheduele.AppointmentDurationInMiniutes) <= scheduele.EndTime)
                {
                    var IsBooked = (scheduele._appointments!.Any(a => a.IsActive == true &&
                    a.AppointmentTime == currentTime));
                    response.ValidTimesForBooking[currentTime] = !IsBooked;
                    currentTime = currentTime.AddMinutes(scheduele.AppointmentDurationInMiniutes);
                }
                result.Add(response);
            }

            return Result.Success<IEnumerable<ScheduleResponse>>(result);






        }

        public async Task<Result> UpdateAsync(Guid ScheduleId, ScheduleRequest request, CancellationToken cancellationToken = default)
        {
            var schedule = await _unitOfWork._scheduleService.GetByIdAsync(ScheduleId, cancellationToken);

            if (schedule == null)
                return Result.Failure(ScheduleErrors.ScheduleNotFound); 

            var SearchOnConflict = await _db.Set<Doctor_Schedule>().AnyAsync(x => x.Id != ScheduleId && x.ClinicId == schedule.ClinicId && x.WorkDay == request.WorkDay &&
            ((x.StartTime <= request.StartTime && x.EndTime >= request.StartTime) || ((request.StartTime <= x.StartTime
            && request.EndTime >= x.StartTime))));

            if (SearchOnConflict)
                return Result.Failure(ScheduleErrors.ScheduleConflict);

            //update Data
            schedule.WorkDay = request.WorkDay;
            schedule.StartTime = request.StartTime; 
            schedule.EndTime = request.EndTime; 
            //schedule.Fee = request.Fee;
            schedule.AppointmentDurationInMiniutes = request.AppointmentDurationInMiniutes;

            await _unitOfWork.Complete(cancellationToken);

            return Result.Success();
        }
    }
}
