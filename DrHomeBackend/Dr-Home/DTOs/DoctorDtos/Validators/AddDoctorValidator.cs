using FluentValidation;

namespace Dr_Home.DTOs.DoctorDtos.Validators
{
    public class AddDoctorValidator:AbstractValidator<AddDoctorDto>
    {
        public AddDoctorValidator()
        {
            //Full Name
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Must Enter FullName")
                .MaximumLength(100)
                .WithMessage("Maximum Length Is 100");

            //Email 
            RuleFor(x => x.Email).NotEmpty()
                .WithMessage("Email Field Is Required")
                .EmailAddress()
                .WithMessage("The Email Is Not In Correct Format");

            //Gender 
            RuleFor(x => x.Gender)
                .NotEmpty()
                .WithMessage("You Must Enter Gender")
                .Must(ValidateGender);


            //Phone Number 
            RuleFor(x => x.PhoneNumber)
               .Length(11, 11)
               .WithMessage("Phone Number Must be 11 number")
               .Must(ValidatePhoneNumber)
               .WithMessage("Enter Correct Phone Number")
               .When(x => x.PhoneNumber is not null);

            //Password 

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(10)
                .WithMessage("Password Must Be at Least 10 Characters");

            //Confirm Password 

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Confirm Password Must Equal Password");


            //SpecializationId 

            RuleFor(x => x.SpecializationId).Must(ValidateSpecializationId)
                .WithMessage("Doctor Must Has A Specialization");
           

                
        }


        private bool ValidatePhoneNumber(string? phoneNumber)
        {
            if (phoneNumber == null) return false;
            if (phoneNumber[0] != '0' || phoneNumber[1] != '1') return false;

            foreach (var i in phoneNumber)
            {
                if (i < '0' && i > '9') return false;
            }

            return true;
        }

        private bool ValidateGender(string gender)
        {
            if (gender != "Male" && gender != "Female" && gender != "ذكر" && gender !=
                "أنثى") return false;

            return true;
        }

        private bool ValidateSpecializationId(int id)
        {
            return id != 0;
        }

    }
}
