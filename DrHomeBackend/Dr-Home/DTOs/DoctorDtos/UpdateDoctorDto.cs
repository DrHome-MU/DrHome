using System.ComponentModel.DataAnnotations;

namespace Dr_Home.DTOs.DoctorDtos
{
    public class UpdateDoctorDto
    {
        [MaxLength(100)]

        [Required]
        public string FullName { get; set; }

        [Required]
        [MaxLength(10)]
        [RegularExpression("^(Male|Female|male|female|ذكر|أنثى)$",
            ErrorMessage = "The Gender must be male or female")]
        public string Gender { get; set; }

        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        public string Email { get; set; }

        [MaxLength(11)]
        [MinLength(11)]
        [RegularExpression(@"^01[0125][0-9]{8}$", 
            ErrorMessage = "رقم الهاتف غير صحيح، يجب أن يكون 11 رقم ويبدأ بـ 01")]
        public string? PhoneNumber { get; set; } 

        public IFormFile? PersonalPic { get; set; }

        public int? SpecilazationId { get; set; }

        public string? Summary { get; set; }

        public string? city { get; set; }


        public string? region { get; set; }

        public DateOnly? DateOfBirth { get; set; }

    }
}
