

namespace Dr_Home.Services.services
{
    public class ScheduleService(AppDbContext db) : IScheduleService
    {
        private readonly AppDbContext _db = db;

        public async Task<Doctor_Schedule> AddAsync(Doctor_Schedule schedule , CancellationToken cancellationToken = default)
        {
            await _db.Set<Doctor_Schedule>().AddAsync(schedule, cancellationToken);

            return schedule;
        }

        public  async Task<Doctor_Schedule> DeleteAsync(Doctor_Schedule doctor_schedule)
        {
            _db.Remove(doctor_schedule);
            return doctor_schedule; 
        }

        public async Task<Doctor_Schedule> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var schedule = await _db.Set<Doctor_Schedule>().Include(x=>x._appointments).FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
            return schedule;
        }

        public async Task<bool> SearchOnConflict(Guid ClinicId, ScheduleRequest request)
        {
            return await _db.Set<Doctor_Schedule>().AnyAsync(x => x.ClinicId == ClinicId && x.WorkDay == request.WorkDay &&
            ((x.StartTime <= request.StartTime && x.EndTime >= request.StartTime) || ((request.StartTime <= x.StartTime
            && request.EndTime >= x.StartTime))));
        }
    }
}
