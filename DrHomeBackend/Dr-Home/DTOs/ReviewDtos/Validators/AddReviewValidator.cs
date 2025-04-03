using FluentValidation;

namespace Dr_Home.DTOs.ReviewDtos.Validators
{
    public class AddReviewValidator:AbstractValidator<AddReviewDto>
    {
        public AddReviewValidator()
        {
            RuleFor(x => x.rating)
                .Must((request, context) => request.rating >= 1 && request.rating <= 5)
                .WithMessage("Rating Must In Range Between 1 And 5");
        }
    }
}
