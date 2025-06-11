using Dr_Home.Authentication;
using Dr_Home.Data.Models;
using Dr_Home.DTOs.AuthDTOs;
using Dr_Home.DTOs.DoctorDtos;
using Dr_Home.DTOs.EmailSender;
using Dr_Home.Email_Sender;
using Dr_Home.Helpers.Interfaces;
using Dr_Home.UnitOfWork;
using Hangfire;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Dr_Home.Helpers.helpers
{
    public class AuthHelper(IUnitOfWork _unitOfWork ,IEmailSender _sender
        ,IJwtProvider jwtProvider) : IAuthHelper
    {
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task<ApiResponse<Patient>> RegisterPatient(RegisterDto dto)
        {
            var HashPass = HashPassword(dto.Password);
            if(await _unitOfWork._userService.IsEmailExists(dto.Email))
            {
                return new ApiResponse<Patient>
                {
                    Success = false, 
                    Message = "Email is in use by another user",
                    Data = null
                };
            }

            var patient = new Patient
            {
                FullName = dto.FullName, 
                Gender = dto.Gender,
                Email = dto.Email,
                HashPassword = HashPass,
                PhoneNumber = dto.PhoneNumber,
                role = "Patient",
                DateOfBirth = dto.DateOfBirth,
            };

              await _unitOfWork._patientService.AddAsync(patient);
              await  _unitOfWork.Complete();

            await SendVerfiyCodeAsync(patient, "VerficationCode");

            return new ApiResponse<Patient>
            {
                Success = true,
                Message = "Register Is Done Successfully",
                Data = patient
            };

        }
        public async Task SendVerfiyCodeAsync(User user , string template)
        {
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();

            var emailBody = EmailBodyBuilder.GenerateEmailBody(template,
             templateModel: new Dictionary<string, string>
             {
                  { "{{Name}}" , $"{user.FullName}" },
                       {"{{otp_code}}" , $"{code}" }

             });

            if (template == "ResetPassword")
            {
                var sendDto = new SendEmailRegisterDto
                {
                    toEmail = user.Email,
                    message = emailBody,
                    subject = "Your Reset Password Code (Valid For 5 Minutes)"
                };
                await _sender.SendRegisterEmailAsync(sendDto);
                user.HashForgetPasswordCode = HashPassword(code);
                user.ForgetPasswordCodeExpiryTime = DateTime.UtcNow.AddMinutes(5);
            }
            else
            {
                var sendDto = new SendEmailRegisterDto
                {
                    toEmail = user.Email,
                    message = emailBody,
                    subject = "Your Verfication Code (Valid For 5 Minutes)"
                };
                await _sender.SendRegisterEmailAsync(sendDto);
                user.HashVerficationCode = HashPassword(code);
                user.VerficationCodeExpiryTime = DateTime.UtcNow.AddMinutes(5);
            }
            await _unitOfWork.Complete();
        }
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }


        public async Task<ApiResponse<User>> LogIn(LogInDto dto)
        {
           var user = await _unitOfWork._userService.GetByEmail(dto.Email);
           if(user == null)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "Email Or Password Are Wrong",
                    Data = null
                };
            }
           

            if (!VerifyPassword(dto.Password , user.HashPassword))
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "Email Or Password Are Wrong"
                };
            }
           //if(user.IsActive == false)
           // {
           //     return new ApiResponse<User>
           //     {
           //         Success = false,
           //         Message = "Verify Your Account First",
           //         Data = null
           //     };
           // }

          if(user.BannedTo != null && user.BannedTo > DateTime.UtcNow)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = $"Your Account Is Banned To {user.BannedTo}"
                };
            }

            return new ApiResponse<User>
            {
                Success = true,
                Message = "User Signed In Successfully",
                token = _jwtProvider.GenerateToken(user),
                Data = user
            };
        }

        public async Task<Result<ActiveAccountResponse>> VerifyAccount(CheckCodeDto checkCodeDto)
        {
           var user = await _unitOfWork._userService.GetByEmail(checkCodeDto.Email);

            if(user == null)
                return Result.Failure<ActiveAccountResponse>(AuthErrors.WrongVerficationCode);

            if(user.IsActive) 
                return Result.Failure<ActiveAccountResponse>(AuthErrors.AccountAlreadyActive);

            if (user.HashVerficationCode == null || user.VerficationCodeExpiryTime == null)
                return Result.Failure<ActiveAccountResponse>(AuthErrors.WrongVerficationCode);

            if (!VerifyPassword(checkCodeDto.Code, user.HashVerficationCode) || user.VerficationCodeExpiryTime < DateTime.UtcNow)
                return Result.Failure<ActiveAccountResponse>(AuthErrors.WrongVerficationCode); 

            user.IsActive = true;
            user.HashVerficationCode = null;
            user.VerficationCodeExpiryTime = null;

            var response = new ActiveAccountResponse
            {
                Email = user.Email,
                UserId = user.Id,
                Token = _jwtProvider.GenerateToken(user)
            };

            await _unitOfWork.Complete();

            return Result.Success(response);
        }

        public async Task<ApiResponse<IEnumerable<User>>> GetUsers()
        {
            var users = await _unitOfWork._userService.GetAllAsync();

            if (!users.Any()) return new ApiResponse<IEnumerable<User>>
            {
                Success = false,
                Message = "There is no users"
            };


            return new ApiResponse<IEnumerable<User>>
            {
                Success = true,
                Message = "Data Loaded Successfully",
                Data = users
            };
        }

        public async Task<ApiResponse<User>> PanUser(Guid id , int numOfPanDays)
        {
            
            var user = await _unitOfWork._userService.GetById(id);

            if (user == null)
            {
                return new ApiResponse<User> { Success = false, Message = "Users Doesn`t Exist" };
            }

           user.BannedTo = DateTime.UtcNow.AddDays(numOfPanDays);
           // await _unitOfWork._userService.DeleteAsync(user);
            await  _unitOfWork.Complete();

            return new ApiResponse<User> { Success = true, Message = $"User Panned For {numOfPanDays} Successfully." };
        }

        public async Task<ApiResponse<User>> GetUser(Guid id)
        {
            var user = await _unitOfWork._userService.GetById(id);


            if (user == null)
            {
                return new ApiResponse<User> { Success = false, Message = "User Not Found" };
            }

            return new ApiResponse<User> { Success = true, Message = "User Found", Data = user };
        }

        public async Task<ApiResponse<UserProfileDto>> GetUserProfile(Guid id)
        {

           var patient = await _unitOfWork._userService.GetById(id);



            if(patient == null)
            {
                return new ApiResponse<UserProfileDto> { 
                    Success = false,
                    Message = "User Doesn`t Exist",
                    Data = null
                };
            } 

            var profile = new UserProfileDto { 
                FullName = patient.FullName,
                Gender = patient.Gender,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                DateOfBirth = patient.DateOfBirth,
                city = patient.city,
                region = patient.region
            };

            return new ApiResponse<UserProfileDto>
            {
                Success = true,
                Message = "The Request Is Successfully Processed",
                Data = profile
            };
        }

        public async Task<ApiResponse<UserProfileDto>> UpdateProfile(Guid id, UserProfileDto dto)
        {
            var user = await _unitOfWork._userService.GetById(id);

            //Handle If User Is Null
            if (user == null)
            {
               return   new ApiResponse<UserProfileDto>
                {
                    Success = false,
                    Message = "User Doesn`t Exist",
                    Data = null
                };
            }
            //Handle if Email In Use

            if(dto.Email != user.Email && await _unitOfWork._userService.IsEmailExists(dto.Email))
            {
                return  new ApiResponse<UserProfileDto>
                {
                    Success = false,
                    Message = "Email is in use by another user",
                    Data = null
                };
            }

            //Update Data 
            user.FullName = dto.FullName;
            user.Gender = dto.Gender;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.DateOfBirth = dto.DateOfBirth;
            user.city = dto.city;
            dto.region = dto.region;

            await _unitOfWork._userService.UpdateAsync(user);
            await  _unitOfWork.Complete();
            
            return new ApiResponse<UserProfileDto>
            {
                Success = true,
                Message = "Data Updated Successfully",
                Data = dto
            };
        }

        public async Task<ApiResponse<User>> ChangePassword(Guid id, ChangePasswordDto dto)
        {
           var user = await _unitOfWork._userService.GetById(id);

            if (user == null)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "المستخدم غير موجود بالنظام"
                };
            }

            if(dto.NewPassword != dto.ConfirmNewPassword)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "تأكد من تأكيد كلمة المرور بشكل صحيح"
                };
            }
            var hashPassword = HashPassword(dto.NewPassword);

            user.HashPassword = hashPassword;

            await _unitOfWork.Complete();

            return new ApiResponse<User>
            {
                Success = true, 
                Message = "تم تعديل كلمة المرور بنجاح"
            };
        }

        public async Task<Result> ForgetPassword(ForgetPasswordDto dto)
        {
            if (await _unitOfWork._userService.GetByEmail(dto.Email) is not { } user)
                return Result.Success();

            await SendVerfiyCodeAsync(user, "ResetPassword"); 

            return Result.Success();    
        }

        public async Task<GetAllUsersResponse> GetAllUsers()
        {
            var patients = await _unitOfWork._patientService.GetAllAsync();
            var doctors = await _unitOfWork._doctorService.GetAllAsync();

            var result = new GetAllUsersResponse
            {
                Doctors = doctors.Adapt<IEnumerable<ShowDoctorDataDto>>(), 
                Patients = patients.Adapt<IEnumerable<UserProfileDto>>(),
                NumberOfDoctors = doctors.Count(),
                NumberOfPatients = patients.Count()
            };  
           
            
           return result;   
        }

        public async Task<bool> EnableUser(Guid id)
        {
            var user = await _unitOfWork._userService.GetById(id);

            if (user == null)
                return false;

            user.BannedTo = null;

            await _unitOfWork.Complete();

            return true;

        }

        public async Task<Result> ResendVerifcationCode(ResendVerficationCodeDto dto)
        {
            if (await _unitOfWork._userService.GetByEmail(dto.Email) is not { } user)
                return Result.Success();

            if (user.IsActive)
                return Result.Failure(AuthErrors.AccountAlreadyActive);

            await SendVerfiyCodeAsync(user, "VerficationCode");

            return Result.Success();

        }

        public async Task<Result> ResetPassword(ResetPasswordDto dto)
        {
            var user = await _unitOfWork._userService.GetByEmail(dto.Email);

            if(user == null || !user.IsActive)
                return Result.Failure(AuthErrors.WrongForgetPasswordCode);

            if (user.HashForgetPasswordCode == null || user.ForgetPasswordCodeExpiryTime == null)
                return Result.Failure<ActiveAccountResponse>(AuthErrors.WrongForgetPasswordCode);

            if (!VerifyPassword(dto.Code, user.HashForgetPasswordCode) || user.ForgetPasswordCodeExpiryTime < DateTime.UtcNow)
                return Result.Failure<ActiveAccountResponse>(AuthErrors.WrongForgetPasswordCode);

            var hasPassword = HashPassword(dto.NewPassword);

            user.HashPassword = hasPassword;


            await _unitOfWork.Complete(); 

            return Result.Success();    
        }
    }
}
