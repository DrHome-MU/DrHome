namespace Dr_Home.Services.Interfaces
{
    public interface IScheduleService
    {

        Task<Doctor_Schedule>AddAsync(Doctor_Schedule schedule , CancellationToken cancellationToken = default);

        Task<bool>SearchOnConflict(Guid ClinicId , ScheduleRequest request);

        Task<Doctor_Schedule> GetByIdAsync(Guid id , CancellationToken cancellationToken = default);    

        Task<Doctor_Schedule> DeleteAsync(Doctor_Schedule doctor_schedule);
    }
}
