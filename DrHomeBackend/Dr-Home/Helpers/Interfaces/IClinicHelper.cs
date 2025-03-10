using Dr_Home.Data.Models;
using Dr_Home.DTOs.ClinicDtos;
using Dr_Home.Helpers.helpers;

namespace Dr_Home.Helpers.Interfaces
{
    public interface IClinicHelper
    {
        Task<ApiResponse<Clinic>> AddClinic(AddClinicDto dto);


        Task<ApiResponse<IEnumerable<Clinic>>> GetDoctorClinics(Guid DoctorId);


        Task<ApiResponse<Clinic>> UpdateDoctorClinic(Guid DoctorId , UpdateClinicDto dto);


        Task<ApiResponse<IEnumerable<Clinic>>> GetAllClincs();

        Task<ApiResponse<Clinic>> DeleteClinic(Guid id);
    }
}
