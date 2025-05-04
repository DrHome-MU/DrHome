using Dr_Home.Data.Models;
using Dr_Home.DTOs.ClinicDtos;
using Dr_Home.Helpers.Interfaces;
using Dr_Home.UnitOfWork;
using Mapster;
using System.Runtime.InteropServices;

namespace Dr_Home.Helpers.helpers
{
    public class ClinicHelper(IUnitOfWork _unitOfWork , IScheduleHelper scheduleHelper) : IClinicHelper
    {
        private readonly IScheduleHelper _scheduleHelper = scheduleHelper;

        public async Task<ApiResponse<ClinicResponseDto>> AddClinic(AddClinicDto dto)
        {
            var doctor = await _unitOfWork._doctorService.GetById(dto.DoctorId);

            if(doctor == null)
            {
                return new ApiResponse<ClinicResponseDto>
                {
                    Success = false,
                    Message = "Doctor Doesn`t Exist"
                };
            }
            if (await _unitOfWork._clinicalService.
                GetClinicByNameAndCityAndRegion(dto.ClinicName, dto.city, dto.region) != null)
            {
                return new ApiResponse<ClinicResponseDto> { Success = false, Message = "Clinic Already Added" };
            }
               
            var clinic = dto.Adapt<Clinic>();

            await _unitOfWork._clinicalService.AddClinicAsync(clinic);
            await _unitOfWork.Complete();

            var result = clinic.Adapt<ClinicResponseDto>();

            return new ApiResponse<ClinicResponseDto>
            {
                Success = true,
                Message = "Clinic Added Successfully",
                Data = result
            };
        }

        public async Task<ApiResponse<Clinic>> DeleteClinic(Guid id)
        {
            var clinic = await _unitOfWork._clinicalService.GetById(id);

            if (clinic == null) return new ApiResponse<Clinic> { Success = false, Message = "Not Found" };
            foreach(var item in clinic._schedules!.ToList())
            {
                await _scheduleHelper.DeleteAsync(item.Id);
            }
            await _unitOfWork._clinicalService.DeleteClinicAsync(clinic);
            await _unitOfWork.Complete();

            return new ApiResponse<Clinic> { Success = true, Message = "Deleted Successfully" };
        }

        public async Task<ApiResponse<IEnumerable<ClinicResponseDto>>> GetAllClincs()
        {
            var clinics = await _unitOfWork._clinicalService.GetAllClinicAsync();

            if (!clinics.Any()) return new ApiResponse<IEnumerable<ClinicResponseDto>> 
            { Success = false, Message = "there is no clinics" };

            var result = clinics.Adapt<IEnumerable<ClinicResponseDto>>();

            return new ApiResponse<IEnumerable<ClinicResponseDto>>
            {
                Success = true,
                Message = "Clinics Loading Done Successfully",
                Data = result
            };
        }

        public async Task<ApiResponse<IEnumerable<ClinicResponseDto>>> GetDoctorClinics(Guid DoctorId)
        {
            var clinics = await _unitOfWork._clinicalService.GetDoctorClinicsAsync(DoctorId);

            var result = clinics.Adapt<IEnumerable<ClinicResponseDto>>();

            if (clinics.Count() == 0)
            {
                return new ApiResponse<IEnumerable<ClinicResponseDto>>
                {
                    Success = false,
                    Message = "There Is No Clinics For This Doctor",
                    Data = result
                };
            }

            return new ApiResponse<IEnumerable<ClinicResponseDto>>
            {
                Success = true,
                Message = "Doctor`s Clinics Loaded Successfully",
                Data = result
            };
        }

        public async Task<ApiResponse<ClinicResponseDto>> UpdateDoctorClinic(Guid ClinicId, UpdateClinicDto dto)
        {
            var clinic = await _unitOfWork._clinicalService.GetById(ClinicId);

            if(clinic == null)
            {
                return new ApiResponse<ClinicResponseDto>
                {
                    Success = false,
                    Message = "The Clinic Is Not Found"
                };
            }

            if(dto.DoctorId != clinic.DoctorId)
            {
                return new ApiResponse<ClinicResponseDto> { Success = false, Message = "Unauthorized" };
            }

            //update data

            clinic.ClinicName = dto.ClinicName;
            clinic.city = dto.city;
            clinic.region = dto.region;
            clinic.PhoneNumber = dto.PhoneNumber;
            clinic.AppointmentFee = dto.AppointmentFee;
            clinic.DetailedAddress = dto.DetailedAddress;

            await _unitOfWork._clinicalService.UpdateClinic(clinic);
            await _unitOfWork.Complete();

            var result = clinic.Adapt<ClinicResponseDto>();

            return new ApiResponse<ClinicResponseDto>
            {
                Success = true,
                Message = "Clinic Updated Successfully",
                Data = result
            };
        }
    }
}
