using Dr_Home.Data.Models;
using Dr_Home.DTOs.AuthDTOs;
using Dr_Home.Helpers.helpers;
using System.Security.Claims;

namespace Dr_Home.Helpers.Interfaces
{
    public interface IAuthHelper
    {
        Task<ApiResponse<Patient>> RegisterPatient(RegisterDto dto);

        string HashPassword(string password);

        bool VerifyPassword(string password, string hashedPassword);
       

        Task<ApiResponse<User>> LogIn(LogInDto dto);

        Task<Result<ActiveAccountResponse>> VerifyAccount(CheckCodeDto checkCodeDto);

        Task<Result> ResendVerifcationCode(ResendVerficationCodeDto dto);

        Task<ApiResponse<IEnumerable<User>>> GetUsers();

        Task<ApiResponse<UserProfileDto>> GetUserProfile(Guid id);

        Task<ApiResponse<UserProfileDto>> UpdateProfile(Guid id , UserProfileDto dto);

        Task<ApiResponse<User>> PanUser(Guid id , int numOfPanDays);

        Task<ApiResponse<User>> GetUser(Guid id);

        Task<ApiResponse<User>>ChangePassword(Guid id , ChangePasswordDto dto);
        Task<Result> ForgetPassword(ForgetPasswordDto dto);

        Task<Result> ResetPassword(ResetPasswordDto dto);

        Task<GetAllUsersResponse> GetAllUsers();

        Task<bool> EnableUser(Guid id);
    }
}
