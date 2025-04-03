using Dr_Home.Data.Models;
using Dr_Home.DTOs.ClinicDtos;
using Dr_Home.Helpers.helpers;

namespace Dr_Home.Helpers.Interfaces
{
    public interface IClinicHelper
    {
        Task<ApiResponse<ClinicResponseDto>> AddClinic(AddClinicDto dto);


        Task<ApiResponse<IEnumerable<ClinicResponseDto>>> GetDoctorClinics(Guid DoctorId);


        Task<ApiResponse<ClinicResponseDto>> UpdateDoctorClinic(Guid ClinicId , UpdateClinicDto dto);


        Task<ApiResponse<IEnumerable<ClinicResponseDto>>> GetAllClincs();

        Task<ApiResponse<Clinic>> DeleteClinic(Guid id);
    }
}
