namespace Dr_Home.DTOs.SupportDtos
{
    public class ResponseToMessageRequestValidator : AbstractValidator<ResponseToMessageRequest>
    {
        public ResponseToMessageRequestValidator()
        {
            RuleFor(x => x.ResponseBody)
              .NotEmpty()
              .WithMessage("The Message Must be Not Empty")
              .NotNull()
              .WithMessage("The Message Must be Not Null");

        }
    }
}
