using FluentValidation;

namespace Dr_Home.DTOs.ReviewDtos.Validators
{
    public class UpdateReviewValidator:AbstractValidator<UpdateReviewDto>
    {
        public UpdateReviewValidator()
        {
            RuleFor(x => x.rating)
              .Must((request, context) => request.rating >= 0 && request.rating <= 5)
              .WithMessage("Rating Must In Range Between 0 And 5");
        }
    }
}
