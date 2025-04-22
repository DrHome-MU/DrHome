
namespace Dr_Home.BackgroundJobs
{
    public class ManageSchedules(AppDbContext db , ILogger<ManageSchedules> logger) : IManageSchedules
    {
        private readonly AppDbContext _db = db;
        private readonly ILogger<ManageSchedules> _logger = logger;

        public async Task DeleteExpiredSchedules()
        {
            var schedules = await _db.Set<Doctor_Schedule>()
                .Include(s => s._appointments)
                .Where(s => s.WorkDay < DateOnly.FromDateTime(DateTime.UtcNow))
                .ToListAsync();
            int count = schedules.Count;
            //_logger.LogInformation("Schedule Count = {count}", count);
            if (schedules.Count > 0)
            {
                foreach (var schedule in schedules)
                {
                    var appointments = schedule._appointments; 
                    foreach(var item in appointments!)
                    {
                        item.IsActive = false;
                        item.ScheduleId = null;
                    }
                    _db.Remove(schedule);
                }
                await _db.SaveChangesAsync();
            }

        }
    }
}
