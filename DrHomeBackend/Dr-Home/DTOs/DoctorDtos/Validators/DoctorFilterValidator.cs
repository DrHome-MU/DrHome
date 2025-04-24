namespace Dr_Home.DTOs.DoctorDtos.Validators
{
    public class DoctorFilterValidator:AbstractValidator<DoctorFilterDto>
    {
        public DoctorFilterValidator()
        {
            //Full Name
            RuleFor(x => x.FullName)
                .MaximumLength(100)
                .WithMessage("Maximum Length Is 100")
                .When(x => x.FullName is not null);




        }
    }
}
