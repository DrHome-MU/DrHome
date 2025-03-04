using System.ComponentModel.DataAnnotations;

namespace Dr_Home.DTOs.AuthDTOs
{
    public class UserProfileDto
    {
        [MaxLength(100)]
        [Required]
        public required string FullName { get; set; }

        
        [MaxLength(10)]
        [RegularExpression("^(Male|Female|male|female|ذكر|أنثى)$",
           ErrorMessage = "The Gender must be male or female")]
        public required string Gender { get; set; }

        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        public required string Email { get; set; }

        [MaxLength(11)]
        [MinLength(11)]
        [RegularExpression(@"^01[0125][0-9]{8}$",
            ErrorMessage = "رقم الهاتف غير صحيح، يجب أن يكون 11 رقم ويبدأ بـ 01")]
        public string? PhoneNumber { get; set; }


        public string ? city { get; set; }

        public string ? region { get; set; }

        public DateOnly? DateOfBirth { get; set; }
    }
}
