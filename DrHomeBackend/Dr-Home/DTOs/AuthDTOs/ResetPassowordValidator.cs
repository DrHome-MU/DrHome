namespace Dr_Home.DTOs.AuthDTOs
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.NewPassword)
               .NotEmpty()
               .MinimumLength(10)
               .WithMessage("Password Must Be at Least 10 Characters"); 

            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.Code)
                .NotEmpty();
        }
    }
}
