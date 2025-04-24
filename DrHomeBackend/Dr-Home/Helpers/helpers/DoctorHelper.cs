using Dr_Home.Data.Models;
using Dr_Home.DTOs.AuthDTOs;
using Dr_Home.DTOs.DoctorDtos;
using Dr_Home.DTOs.EmailSender;
using Dr_Home.Email_Sender;
using Dr_Home.Errors;
using Dr_Home.File_Manager;
using Dr_Home.Helpers.Interfaces;
using Dr_Home.UnitOfWork;
using Mapster;
using System.Reflection.Metadata.Ecma335;

namespace Dr_Home.Helpers.helpers
{
    public class DoctorHelper(IUnitOfWork _unitOfWork,IAuthHelper _auth,
        IFileManager _fileManager) : IDoctorHelper
    {
        
        public async Task<ApiResponse<GetDoctorDto>> AddDoctor(AddDoctorDto dto)
        {
            var hashPassword = _auth.HashPassword(dto.Password);

            if(await _unitOfWork._userService.IsEmailExists(dto.Email))
            {
                return new ApiResponse<GetDoctorDto>
                {
                    Success = false,
                    Message = "Email is in use by another user",
                    Data = null
                };
            }
            var doctor = dto.Adapt<Doctor>();
            doctor.HashPassword = hashPassword;
            doctor.role = "Doctor";

            await _unitOfWork._doctorService.AddAsync(doctor);
            await _unitOfWork.Complete();

            var res = doctor.Adapt<GetDoctorDto>();

            return new ApiResponse<GetDoctorDto>
            {
                Success = true, 
                Message = "The Doctor Added Successfully",
                Data = res

            };
        }

        /// Delete Doctor


        public async Task<ApiResponse<Doctor>> DeleteDoctor(Guid id)
        {
            var doctor = await _unitOfWork._doctorService.GetById(id);

            if (doctor == null)
            {
                return new ApiResponse<Doctor>
                {
                    Success = false,
                    Message = "Doctor Doesn`t Exist"
                };
            }

            if (doctor.ProfilePic_Path != null) await _fileManager.Delete(doctor.ProfilePic_Path);

            await _unitOfWork._doctorService.DeleteAsync(doctor);
            await _unitOfWork._userService.DeleteAsync(doctor);

            await _unitOfWork.Complete();

            return new ApiResponse<Doctor>
            {
                Success = true,
                Message = "Doctor Deleted Successfully"
            };

        }

       
        

        /// Update Doctor Data
        public async Task<ApiResponse<Doctor>> UpdateDoctor(Guid userId , UpdateDoctorDto dto, 
            CancellationToken cancellationToken)
        {
            var doctor = await  _unitOfWork._doctorService.GetById(userId);

             if (doctor == null) return new ApiResponse<Doctor>
             {
                 Success = false,
                 Message = "Doctor Does not Exist"
             };

            if (dto.Email != doctor.Email && 
                await _unitOfWork._userService.IsEmailExists(dto.Email))
            {
                return new ApiResponse<Doctor>
                {
                    Success = false, 
                    Message = "Email is in use by another user",
                    Data = doctor

                };
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
           
            
            //Update The Record in the database
            await _unitOfWork._doctorService.UpdateAsync(doctor);
            await  _unitOfWork.Complete(cancellationToken);


            return new ApiResponse<Doctor>
            {
                Success = true,
                Message = "The Data Updated Successfully",
                Data = doctor
            };

        }

        public async Task<Result> UpdateDoctorProfilePicture(UpdatePictureDto dto, CancellationToken cancellationToken = default)
        {
            var doctor = await _unitOfWork._doctorService.GetById(dto.DoctorId);

            if (doctor == null) return Result.Failure(DoctorErrors.DoctorNotFound);

            if (doctor.ProfilePic_Path != null) { await _fileManager.Delete(doctor.ProfilePic_Path); }

            if (dto.PersonalPic == null)
            {

                doctor.ProfilePic_Path = null;
            }

            else doctor.ProfilePic_Path = await _fileManager.Upload(dto.PersonalPic, cancellationToken);

            await _unitOfWork.Complete(cancellationToken);

            return Result.Success();
        }




        /// Get All Doctors
        public async Task<ApiResponse<IEnumerable<GetDoctorDto>>> GetDoctors()
        {
            var doctors = await _unitOfWork._doctorService.GetAllAsync();

            if (!doctors.Any())
            {
                return new ApiResponse<IEnumerable<GetDoctorDto>>
                {
                    Success = false,
                    Message = "There Is No Doctors"
                };
            }
            var result = doctors.Adapt<IEnumerable<GetDoctorDto>>();

            return new ApiResponse<IEnumerable<GetDoctorDto>>
            {
                Success = true,
                Message = "Loading Doctors Done Successfully",
                Data = result
            };
        }
        ///Show Doctor Profile Data
        public async Task<ApiResponse<ShowDoctorDataDto>> ShowDoctorData(Guid id)
        {
            var doctor = await _unitOfWork._doctorService.GetById(id);

            if (doctor == null)
                return new ApiResponse<ShowDoctorDataDto> { Success = false, Message = "Doctor Doesn`t Exist" };

            var result = doctor.Adapt<ShowDoctorDataDto>();

            if(doctor._specialization != null)result.specialization = doctor._specialization.Name;

            return new ApiResponse<ShowDoctorDataDto> { Success = true, Message = "Done!", Data = result };
        }

        public async Task<Result<IEnumerable<GetDoctorDto>>> FilterDoctors(DoctorFilterDto doctorFilterDto, CancellationToken cancellationToken = default)
        {
            var doctors = await _unitOfWork._doctorService.FilterDoctorAsync(doctorFilterDto, cancellationToken);

            return Result.Success(doctors);
        }
    }
}
