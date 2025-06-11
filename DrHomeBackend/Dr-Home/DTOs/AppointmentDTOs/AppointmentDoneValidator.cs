namespace Dr_Home.DTOs.AppointmentDTOs
{
    public class AppointmentDoneValidator:AbstractValidator<AppointmentDoneRequest>
    {
        public AppointmentDoneValidator()
        {
            RuleFor(x => x.AppointmentDetails)
                .NotNull()
                .WithMessage("AppointmentDetails Can`t be Null");
        }
    }
}
