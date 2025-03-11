using Dr_Home.Data.Models;
using Dr_Home.DTOs.AuthDTOs;
using Dr_Home.DTOs.EmailSender;
using Dr_Home.Email_Sender;
using Dr_Home.Helpers.Interfaces;
using Dr_Home.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Dr_Home.Helpers.helpers
{
    public class AuthHelper(IUnitOfWork _unitOfWork , jwtOptions jwt,IEmailSender _sender) : IAuthHelper
    {
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

            var tokenParameters = new CreateTokenDto
            {
                FullName = patient.FullName,
                Id = patient.Id,
                role = patient.role
            };

            string token = await CreateJwtToken(tokenParameters);
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

            await _sender.SendRegisterEmailAsync(sendDto);
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

        public async Task<string> CreateJwtToken(CreateTokenDto parameters)
        {
            var authClaims = new Claim[]
           {
               new Claim(ClaimTypes.NameIdentifier , parameters.Id.ToString()),
               new Claim(ClaimTypes.Name , parameters.FullName) ,
               new Claim(ClaimTypes.Role,parameters.role)
               // new Claim(ClaimTypes.Email,parameters.Email)
           };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));


            var token = new JwtSecurityToken
            (
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: authClaims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: new SigningCredentials(key,
                SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using var generator = RandomNumberGenerator.Create();

            generator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public async Task<ClaimsPrincipal?> GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(jwt.Key);//the key of our jwt token
            var validition = new TokenValidationParameters
            {
                ValidIssuer = jwt.Issuer,
                ValidAudience = jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false // we must make it false because will compare with expired token
            };

            return new JwtSecurityTokenHandler().ValidateToken(token, validition, out _);
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
            var tokenParams = new CreateTokenDto
            {
                Id = user.Id,
                role = user.role,
                FullName = user.FullName
            };
            return new ApiResponse<User>
            {
                Success = true,
                Message = "User Signed In Successfully",
                token = await CreateJwtToken(tokenParams),
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
                var reviews = await _unitOfWork._reviewService.GetPatientReviews(user.Id);

                foreach (var review in reviews)
                {
                    await _unitOfWork._reviewService.DeleteAsync(review);
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
            var tokenParameters = new CreateTokenDto
            {
                Email = dto.Email
            };
            string token = await CreateJwtToken(tokenParameters);
            string appUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "https://localhost:3000";
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

            return "a7a";
        }
    }
}
