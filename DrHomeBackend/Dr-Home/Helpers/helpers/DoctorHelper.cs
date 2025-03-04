using Dr_Home.Data.Models;
using Dr_Home.DTOs.AuthDTOs;
using Dr_Home.DTOs.DoctorDtos;
using Dr_Home.DTOs.EmailSender;
using Dr_Home.Email_Sender;
using Dr_Home.Helpers.Interfaces;
using Dr_Home.UnitOfWork;

namespace Dr_Home.Helpers.helpers
{
    public class DoctorHelper(IUnitOfWork _unitOfWork,IAuthHelper _auth,IEmailSender _sender) : IDoctorHelper
    {
        public async Task<ApiResponse<Doctor>> AddDoctor(AddDoctorDto dto)
        {
            var hashPassword = _auth.HashPassword(dto.Password);

            if(await _unitOfWork._userService.IsEmailExists(dto.Email))
            {
                return new ApiResponse<Doctor>
                {
                    Success = false,
                    Message = "Email is in use by another user",
                    Data = null
                };
            }

            var doctor = new Doctor
            {
                FullName = dto.FullName,
                Gender = dto.Gender,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                city = dto.city,
                region = dto.region,
                HashPassword = hashPassword,
                role = dto.role

            };

            await _unitOfWork._doctorService.AddAsync(doctor);
            _unitOfWork.Complete();

            var tokenParameters = new CreateTokenDto
            {
                FullName = doctor.FullName,
                Id = doctor.Id,
                role = doctor.role
            };

            string token = await _auth.CreateJwtToken(tokenParameters);

            string appUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "https://localhost:3000";
            string link = $"{appUrl}/api/auth/verify?token={token}";
           

            string html_tmp = $@"
            <div>
                <p>Click on the link below to verify your account</p>
                <a href='{link}'>Verify</a>
            </div>";


            var sendDto = new SendEmailRegisterDto
            {
                toEmail = doctor.Email,
                subject = "Dr Home Verfication",
                message = html_tmp
            };

            await _sender.SendRegisterEmailAsync(sendDto);

            return new ApiResponse<Doctor>
            {
                Success = false, 
                Message = "The Doctor Added Successfully",
                Data = doctor

            };
        }

        public async Task<Doctor> DeleteDoctor(Guid id)
        {
            var doctor = await _unitOfWork._doctorService.GetById(id); 

            if(doctor != null)
            {
                var ret = await _unitOfWork._doctorService.DeleteAsync(doctor);
                await _unitOfWork._userService.DeleteAsync(doctor);
                _unitOfWork.Complete();
                return ret;
            }
            return null;
        }

        public async Task DeletePic(Guid id, string uploadPath)

        {
           string  idString = id.ToString();
            var oldPic = Directory.GetFiles(uploadPath, $"{idString}.*");

            foreach (var file in oldPic)
            {
                File.Delete(file);
            }
          
        }

        public async Task<ApiResponse<Doctor>> UpdateDoctor(Guid userId , UpdateDoctorDto dto)
        {
            var doctor = await  _unitOfWork._doctorService.GetById(userId);

             if (doctor == null) return new ApiResponse<Doctor>
             {
                 Success = false,
                 Message = "Doctor Does not Exist"
             };

            if (dto.Email != doctor.Email && 
                !await _unitOfWork._userService.IsEmailExists(dto.Email))
            {
                return new ApiResponse<Doctor>
                {
                    Success = false, 
                    Message = "Email is in use by another user",
                    Data = doctor

                };
            }
            

           

            var uploadPath  = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Pictures/Doctors");

            if (dto.PersonalPic != null)
            {
                var param = new UpdatePictureDto
                {
                    uploadPath = uploadPath,
                    pic = dto.PersonalPic,
                    Id = userId
                };
                doctor.ProfilePic_Path = await UpdateDoctorPic(param);
            }
            else
            {
                await DeletePic(userId, uploadPath);
                doctor.ProfilePic_Path= null;
            }
            //update the doctor details
            doctor.FullName = dto.FullName; 
            doctor.Summary = dto.Summary;
            doctor.PhoneNumber = dto.PhoneNumber;
            doctor.Gender = dto.Gender;
            doctor.city = dto.city; 
            doctor.region = dto.region;
            doctor.Email = dto.Email; 
            doctor.DateOfBirth = dto.DateOfBirth;
            doctor.SpecializationId = dto.SpecilazationId;
            
            //Update The Record in the database
            await _unitOfWork._doctorService.UpdateAsync(doctor);
            _unitOfWork.Complete();


            return new ApiResponse<Doctor>
            {
                Success = true,
                Message = "The Data Updated Successfully",
                Data = doctor
            };

        }

        public async Task<string> UpdateDoctorPic(UpdatePictureDto dto)
        {
            if (!Directory.Exists(dto.uploadPath)) { Directory.CreateDirectory(dto.uploadPath); }
            //New Pic Info
            var newExtension = Path.GetExtension(dto.pic.FileName);
            string newFileName = $"{dto.Id.ToString()}{newExtension}";
            string newFilePath = Path.Combine(dto.uploadPath, newFileName);

            // delete the old pic , if it exists

            await DeletePic(dto.Id, dto.uploadPath);

            //Save The New Pic 
            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await dto.pic.CopyToAsync(stream);
            }

            return newFileName;

        }
    }
}
