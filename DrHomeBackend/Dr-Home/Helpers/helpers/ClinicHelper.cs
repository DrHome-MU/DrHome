using Dr_Home.Data.Models;
using Dr_Home.DTOs.ClinicDtos;
using Dr_Home.Helpers.Interfaces;
using Dr_Home.UnitOfWork;
using System.Runtime.InteropServices;

namespace Dr_Home.Helpers.helpers
{
    public class ClinicHelper(IUnitOfWork _unitOfWork) : IClinicHelper
    {
        public async Task<ApiResponse<Clinic>> AddClinic(AddClinicDto dto)
        {
            var doctor = await _unitOfWork._doctorService.GetById(dto.DoctorId);

            if(doctor == null)
            {
                return new ApiResponse<Clinic>
                {
                    Success = false,
                    Message = "Doctor Doesn`t Exist"
                };
            }
            var clinic = new Clinic
            {
                DoctorId = dto.DoctorId,
                ClinicName = dto.ClinicName,
                city = dto.city,
                region = dto.region,
                PhoneNumber = dto.PhoneNumber
            };

            await _unitOfWork._clinicalService.AddClinicAsync(clinic);
            await _unitOfWork.Complete();

            return new ApiResponse<Clinic>
            {
                Success = true,
                Message = "Clinic Added Successfully",
                Data = clinic
            };
        }

        public async Task<ApiResponse<Clinic>> DeleteClinic(Guid id)
        {
            var clinic = await _unitOfWork._clinicalService.GetById(id);

            if (clinic == null) return new ApiResponse<Clinic> { Success = false, Message = "Not Found" };

            await _unitOfWork._clinicalService.DeleteClinicAsync(clinic);
            await _unitOfWork.Complete();

            return new ApiResponse<Clinic> { Success = true, Message = "Deleted Successfully" };
        }

        public async Task<ApiResponse<IEnumerable<Clinic>>> GetAllClincs()
        {
            var clinics = await _unitOfWork._clinicalService.GetAllClinicAsync();

            if (!clinics.Any()) return new ApiResponse<IEnumerable<Clinic>> 
            { Success = false, Message = "there is no clinics" };


            return new ApiResponse<IEnumerable<Clinic>>
            {
                Success = true,
                Message = "Clinics Loading Done Successfully",
                Data = clinics
            };
        }

        public async Task<ApiResponse<IEnumerable<Clinic>>> GetDoctorClinics(Guid DoctorId)
        {
            var clinics = await _unitOfWork._clinicalService.GetDoctorClinicsAsync(DoctorId);
            
            Console.WriteLine(clinics.Count());

            if (clinics.Count() == 0)
            {
               return  new ApiResponse<IEnumerable<Clinic>>
                {
                    Success = false,
                    Message = "There Is No Clinics For This Doctor"
                }; 
            }

            return new ApiResponse<IEnumerable<Clinic>>
            {
                Success = true,
                Message = "Doctor`s Clinics Loaded Successfully",
                Data = clinics
            };
        }

        public async Task<ApiResponse<Clinic>> UpdateDoctorClinic(Guid DoctorId, UpdateClinicDto dto)
        {
            var clinic = await _unitOfWork._clinicalService.GetById(dto.ClinicId);

            if(clinic == null)
            {
                return new ApiResponse<Clinic>
                {
                    Success = false,
                    Message = "The Clinic Is Not Found"
                };
            }

            if(DoctorId != clinic.DoctorId)
            {
                return new ApiResponse<Clinic> { Success = false, Message = "Unauthorized" };
            }

            //update data

            clinic.ClinicName = dto.ClinicName;
            clinic.PhoneNumber = dto.PhoneNumber;
            clinic.region = dto.region; 
            clinic.city = dto.city;

            await _unitOfWork._clinicalService.UpdateClinic(clinic);
            await _unitOfWork.Complete();

            return new ApiResponse<Clinic>
            {
                Success = true,
                Message = "Clinic Updated Successfully",
                Data = clinic
            };
        }
    }
}
