namespace Dr_Home.DTOs.DoctorDtos
{
    public class GetDoctorDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }  = string.Empty;


        public string Gender { get; set; } = string.Empty;


        public string Email { get; set; } = string.Empty;


        public string? PhoneNumber { get; set; }

        public string? ProfilePic_Path { get; set; }

        public int? SpecializationId { get; set; }

        public string? Summary { get; set; }

        public string? city { get; set; }


        public string? region { get; set; }

        public DateOnly? DateOfBirth { get; set; }
        public DateTime? BannedTo { get; set; }
    }
}
