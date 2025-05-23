﻿using Dr_Home.Data.Models;
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

        Task<bool> VerifyAccount(string token);

        Task<ApiResponse<IEnumerable<User>>> GetUsers();

        Task<ApiResponse<UserProfileDto>> GetUserProfile(Guid id);

        Task<ApiResponse<UserProfileDto>> UpdateProfile(Guid id , UserProfileDto dto);

        Task<ApiResponse<User>> DeleteUser(Guid id);

        Task<ApiResponse<User>> GetUser(Guid id);

        Task<ApiResponse<User>>ChangePassword(Guid id , ChangePasswordDto dto);
        Task<string> ForgetPassword(forgotPasswordDto dto);
    }
}
