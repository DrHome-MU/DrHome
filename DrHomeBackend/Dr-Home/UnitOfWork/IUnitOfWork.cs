using Dr_Home.Data.Models;
using Dr_Home.Services.Interfaces;

namespace Dr_Home.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        
        IPatientService _patientService { get; }

        IDoctorService _doctorService { get; }

        IUserService _userService { get; }

        IReviewService _reviewService { get; }

        IClinicService _clinicalService { get; }

        ISpecializationService _specializationService { get; }
        IScheduleService _scheduleService { get; }
        IAppointmentService _appointmentService { get; }

        ICityService _cityService { get; }  

        IRegionService _regionService { get; }
        Task<int> Complete(CancellationToken cancellationToken = default);
    }
}
