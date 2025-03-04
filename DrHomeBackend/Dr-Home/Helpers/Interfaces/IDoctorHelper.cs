using Dr_Home.Data.Models;
using Dr_Home.DTOs.DoctorDtos;
using Dr_Home.Helpers.helpers;

namespace Dr_Home.Helpers.Interfaces
{
    public interface IDoctorHelper
    {
        Task<ApiResponse<Doctor>>AddDoctor(AddDoctorDto dto);
        Task<Doctor>DeleteDoctor(Guid id);
        Task<ApiResponse<Doctor>>UpdateDoctor(Guid userId , UpdateDoctorDto dto);
        Task<string> UpdateDoctorPic(UpdatePictureDto dto);

        Task  DeletePic(Guid id, string uploadPath);
    }
}
