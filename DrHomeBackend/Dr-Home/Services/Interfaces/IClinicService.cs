using Dr_Home.Data.Models;

namespace Dr_Home.Services.Interfaces
{
    public interface IClinicService
    {
        Task<Clinic>AddClinicAsync(Clinic clinic);


        Task<IEnumerable<Clinic>> GetAllClinicAsync();

        Task<IEnumerable<Clinic>> GetDoctorClinicsAsync(Guid DoctorId);

        Task<Clinic>UpdateClinic(Clinic clinic);


        Task<Clinic>GetById(Guid id);   


        Task<Clinic>DeleteClinicAsync(Clinic clinic);
    }
}
