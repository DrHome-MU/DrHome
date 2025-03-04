using System.ComponentModel.DataAnnotations;

namespace Dr_Home.DTOs.DoctorDtos
{
    public class AddDoctorDto
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

        public string city { get; set; }

        public string region { get; set; }

        [Required]
        [MinLength(10)]


        public string Password { get; set; }

        [Required]
        [MinLength(10)]

        public string ConfirmPassword { get; set; }
        [MaxLength(20)]
        [RegularExpression("^(Admin|Doctor|Patient|أدمن|مريض|دكتور)$",
           ErrorMessage = "The type must be either Admin Or Patient Or Doctor.")]
        public string role { get; set; }
    }
}
