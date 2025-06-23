namespace Dr_Home.DTOs.AppointmentDTOs
{
    public class DoctorAppointmentsDeatailsValidator:AbstractValidator<DoctorAppointmentsDetailsResponse>
    {
        public DoctorAppointmentsDeatailsValidator()
        {
            RuleFor(x=> x.PatientName)
                .NotEmpty()
                .WithMessage("Must Enter Doctor Name")
                .MaximumLength(100)
                .WithMessage("Maximum Length Is 100");


            RuleFor(x => x.AppointmentDate)
               .NotEmpty()
               .WithMessage("The Work Day Is Required");

            RuleFor(x => x.AppointmentTime)
                .NotEmpty();
              
        }
    }
}
