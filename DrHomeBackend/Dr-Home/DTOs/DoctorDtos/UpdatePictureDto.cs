namespace Dr_Home.DTOs.DoctorDtos
{
    public class UpdatePictureDto
    {
        public Guid Id { get; set; }

        public IFormFile pic {  get; set; } 

        public string uploadPath { get; set; }
    }
}
