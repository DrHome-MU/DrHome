namespace Dr_Home.DTOs.AppointmentDTOs
{
    public class PatientAppointmentsDetailsValidator : AbstractValidator<PatientAppointmentsDetailsResponse>
    {
        public PatientAppointmentsDetailsValidator()
        {
            RuleFor(x => x.DoctorName)
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
