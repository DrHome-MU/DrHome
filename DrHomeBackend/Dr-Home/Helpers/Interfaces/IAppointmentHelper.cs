using Dr_Home.DTOs.AppointmentDTOs;

namespace Dr_Home.Helpers.Interfaces
{
    public interface IAppointmentHelper
    {
        Task<Result<AppointmentResponse>> BookAppointmentAsync(Guid ScheduleId , AppointmentRequest request , CancellationToken cancellationToken = default);

        Task<Result<AppointmentResponse>> GetAppointmentAsync(Guid AppointmentId, CancellationToken cancellationToken = default); 

        Task<Result<IEnumerable<GetDoctorAppointments>>> GetDoctorAppointmentsAsync(Guid DoctorId, CancellationToken cancellationToken = default);

        Task<Result<IEnumerable<GetPatientAppointmentsResponse>>> GetPatientAppointmentsAsync(Guid PatientId , CancellationToken cancellationToken = default);
            
        Task<Result> UpdateAppointmentAsync(Guid AppointmentId,
            AppointmentRequest request, CancellationToken cancellationToken);

        Task<Result> toggleActiveAsync(Guid AppointmentId , CancellationToken cancellationToken = default);

        Task<Result> toggleDoneeAsync(Guid AppointmentId, CancellationToken cancellationToken = default);

        Task<Result<AppointmentIsDoneResponse>> AppointmentIsDoneAsync([FromQuery] Guid PatientId, [FromQuery] Guid DoctorId, CancellationToken cancellationToken = default);
    }
}
