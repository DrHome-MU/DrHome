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
           
            var random = new Random();
            string confimationCode = random.Next(100000, 999999).ToString(); 

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
                ConfirmationCode = confimationCode,
                DateOfBirth = dto.DateOfBirth,
            };

              await _unitOfWork._patientService.AddAsync(patient);
              await  _unitOfWork.Complete();


            string token =  _jwtProvider.GenerateToken(patient);
            string appUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "https://localhost:3000";
            string link = $"{appUrl}/api/auth/verify?token={token}";

            string html_tmp = $@"
            <div>
                <p>Click on the link below to verify your account</p>
                <a href='{link}'>Verify</a>
            </div>";


            var sendDto = new SendEmailRegisterDto
            {
                toEmail = patient.Email,
                subject = "Dr Home Verfication",
                message = html_tmp
            };

            BackgroundJob.Enqueue(() => _sender.SendRegisterEmailAsync(sendDto));

            //await _sender.SendRegisterEmailAsync(sendDto);
            return new ApiResponse<Patient>
            {
                Success = true,
                Message = "Register Is Done Successfully",
                Data = patient
            };

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
          
            return new ApiResponse<User>
            {
                Success = true,
                Message = "User Signed In Successfully",
                token = _jwtProvider.GenerateToken(user),
                Data = user
            };
        }

        public async Task<bool> VerifyAccount(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return false;
            var user = await _unitOfWork._userService.GetById(Guid.Parse(userId));
            if (user == null) return false;
            user.IsActive = true;
            await _unitOfWork.Complete();

            return true;
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

        public async Task<ApiResponse<User>> DeleteUser(Guid id)
        {
            
            var user = await _unitOfWork._userService.GetById(id);

            if (user == null)
            {
                return new ApiResponse<User> { Success = false, Message = "Users Doesn`t Exist" };
            }

            if (user.role == "Patient")
            {
                var patient = await _unitOfWork._patientService.GetById(user.Id);
                
                var reviews = patient.Reviews;
                var appointments = patient._appointments;

                foreach (var review in reviews!)
                {
                    await _unitOfWork._reviewService.DeleteAsync(review);
                }

                foreach(var appointment in appointments!)
                {
                    appointment.IsActive = false;
                    appointment.PatientId = null;
                }
              await  _unitOfWork.Complete();
            }
            await _unitOfWork._userService.DeleteAsync(user);
            await  _unitOfWork.Complete();

            return new ApiResponse<User> { Success = true, Message = "User Deleted Successfully" };
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

        public async Task<string> ForgetPassword(forgotPasswordDto dto)
        {
            var user = await _unitOfWork._userService.GetByEmail(dto.Email);

            if (user == null) { return "user doesn`t exist"; }
          
            string token = _jwtProvider.GenerateToken(user);
            string appUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://dr-home.runasp.net";
            string link = $"{appUrl}/api/auth/ChangePassword?token={token}";
            string html_tmp = $@"
            <div>
                <p>Click on the link below to reset your account password</p>
                <a href='{link}'>Reset</a>
            </div>";
            
            var sendDto = new SendEmailRegisterDto
            {
                toEmail = dto.Email,
                subject = "Dr Home Reset Password",
                message = html_tmp
            };

            await _sender.SendRegisterEmailAsync(sendDto);

            return token;
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
    }
}
