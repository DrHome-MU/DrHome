using Dr_Home.DTOs.SchedulesDTOs;

namespace Dr_Home.Helpers.Interfaces
{
    public interface IScheduleHelper
    {
        Task<Result<ScheduleResponse>> AddScheduleAsync(Guid DoctorId ,  Guid ClinicId, ScheduleRequest request, CancellationToken cancellationToken = default);

        Task<Result<IEnumerable<ScheduleResponse>>> GetSchedulesAsync(Guid DoctorId ,Guid ClinicId, CancellationToken cancellationToken = default);

        Task<Result<ScheduleResponse>>GetSchedueleAsync(Guid ClinicId , Guid ScheduleId , CancellationToken cancellationToken = default);
        
        Task<Result> UpdateAsync(Guid ScheduleId , ScheduleRequest request, CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(Guid ScheduleId , CancellationToken cancellationToken = default);
    }
}
