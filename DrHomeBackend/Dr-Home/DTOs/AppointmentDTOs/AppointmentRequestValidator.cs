namespace Dr_Home.DTOs.AppointmentDTOs
{
    public class AppointmentRequestValidator:AbstractValidator<AppointmentRequest>
    {
        public AppointmentRequestValidator()
        {
            RuleFor(x => x.PatientId).
                NotEmpty()
                .WithMessage("Patient Id Is Required");

            RuleFor(x => x.PatientName)
                .NotEmpty()
                .WithMessage("Patient Name Is Required")
                .Length(2, 100)
                .WithMessage("Patient Name Must be between 2 and 100 characters");

            RuleFor(x => x.PatientPhoneNumber)
                .NotEmpty()
                .WithMessage("Phone Number Is Required")
                .Length(11, 11)
                .WithMessage("Phone Number Must be 11 Number")
                .Must(ValidatePhoneNumber)
               .WithMessage("Enter Correct Phone Number");


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
