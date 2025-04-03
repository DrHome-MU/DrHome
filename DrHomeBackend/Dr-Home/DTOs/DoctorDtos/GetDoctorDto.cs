namespace Dr_Home.DTOs.DoctorDtos
{
    public class GetDoctorDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }


        public string Gender { get; set; }


        public string Email { get; set; }


        public string? PhoneNumber { get; set; }

        public string? ProfilePic_Path { get; set; }

        public int? SpecializationId { get; set; }

        public string? Summary { get; set; }

        public string? city { get; set; }


        public string? region { get; set; }

        public DateOnly? DateOfBirth { get; set; }
    }
}
