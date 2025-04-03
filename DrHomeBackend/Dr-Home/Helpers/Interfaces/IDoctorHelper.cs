using Dr_Home.Abstractions;
using Dr_Home.Data.Models;
using Dr_Home.DTOs.DoctorDtos;
using Dr_Home.Helpers.helpers;
using Newtonsoft.Json.Linq;


namespace Dr_Home.Helpers.Interfaces
{
    public interface IDoctorHelper
    {
        Task<ApiResponse<GetDoctorDto>>AddDoctor(AddDoctorDto dto);

        Task<ApiResponse<Doctor>>DeleteDoctor(Guid id);

        Task<ApiResponse<Doctor>>UpdateDoctor(Guid userId , UpdateDoctorDto dto , 
            CancellationToken cancellationToken);

        Task<Result> UpdateDoctorProfilePicture(UpdatePictureDto dto , CancellationToken cancellationToken = default);

        Task<ApiResponse<IEnumerable<GetDoctorDto>>> GetDoctors();

        Task<ApiResponse<ShowDoctorDataDto>> ShowDoctorData(Guid id);
       


    }
}
