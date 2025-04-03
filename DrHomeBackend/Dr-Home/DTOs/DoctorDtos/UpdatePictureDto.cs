namespace Dr_Home.DTOs.DoctorDtos
{
    public class UpdatePictureDto
    {
        public Guid DoctorId { get; set; }

        public IFormFile? PersonalPic {  get; set; } 

    }
}
