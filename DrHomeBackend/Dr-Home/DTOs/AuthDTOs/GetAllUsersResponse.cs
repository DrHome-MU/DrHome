using Dr_Home.DTOs.DoctorDtos;

namespace Dr_Home.DTOs.AuthDTOs
{
    public class GetAllUsersResponse
    {
        public IEnumerable<ShowDoctorDataDto> Doctors { get; set; } = [];
        public IEnumerable<UserProfileDto> Patients { get; set; } = [];

        public int NumberOfDoctors { get; set; }

        public int NumberOfPatients { get; set; }

    }
}
