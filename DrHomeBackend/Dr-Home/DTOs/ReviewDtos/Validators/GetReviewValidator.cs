using FluentValidation;

namespace Dr_Home.DTOs.ReviewDtos.Validators
{
    public class GetReviewValidator : AbstractValidator<GetReviewDto>
    {
        public GetReviewValidator()
        {
            RuleFor(x => x.rating)
               .Must((request, context) => request.rating >= 1 && request.rating <= 5)
               .WithMessage("Rating Must In Range Between 1 And 5");


            //Full Name
            RuleFor(x => x.ReviwerName)
                .NotEmpty()
                .WithMessage("Must Enter Reviewr Name")
                .MaximumLength(100)
                .WithMessage("Maximum Length Is 100");
        }
    }
}
