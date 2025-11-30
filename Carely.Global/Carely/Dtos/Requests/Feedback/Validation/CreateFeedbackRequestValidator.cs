using FluentValidation;

namespace Carely.Dtos.Requests.Feedback.Validation
{
    public class CreateFeedbackRequestValidator : AbstractValidator<CreateFeedbackRequest>
    {
        public CreateFeedbackRequestValidator()
        {
            RuleFor(x => x.Stars)
                .NotEmpty()
                .WithMessage("You must give at least one star ")
                .InclusiveBetween(1, 5)
                .WithMessage("Stars must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .NotEmpty()
                .WithMessage("you must leave a comment")
                .MaximumLength(500)
                .WithMessage("Comment cannot exceed 500 characters.");
        }
    }

}
