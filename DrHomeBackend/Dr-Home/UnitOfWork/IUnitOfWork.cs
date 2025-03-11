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
        Task<int> Complete();
    }
}
