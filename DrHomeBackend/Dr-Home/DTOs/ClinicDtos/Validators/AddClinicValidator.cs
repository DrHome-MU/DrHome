using FluentValidation;

namespace Dr_Home.DTOs.ClinicDtos.Validators
{
    public class AddClinicValidator:AbstractValidator<AddClinicDto>
    {
        public AddClinicValidator()
        {
            RuleFor(x => x.ClinicName).
                NotEmpty()
                .WithMessage("Clinic Name Is Required");


            RuleFor(x => x.city).
               NotEmpty()
               .WithMessage("The City Is Required");


            RuleFor(x => x.region).
              NotEmpty()
              .WithMessage("The region Is Required");


            //Phone Number 
            RuleFor(x => x.PhoneNumber)
               .Length(11, 11)
               .WithMessage("Phone Number Must be 11 number")
               .Must(ValidatePhoneNumber)
               .WithMessage("Enter Correct Phone Number")
               .When(x => x.PhoneNumber is not null);


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
    }
}
