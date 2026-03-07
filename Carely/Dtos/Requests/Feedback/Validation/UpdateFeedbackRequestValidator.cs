using FluentValidation;

namespace Carely.Dtos.Requests.Feedback.Validation
{
    public class UpdateFeedbackRequestValidator : AbstractValidator<UpdateFeedbackRequest>
    {
        public UpdateFeedbackRequestValidator()
        {
            RuleFor(x => x.Stars)
                .NotEmpty()
                .WithMessage("you must leave at least one star")
                .InclusiveBetween(1, 5)
                .When(x => x.Stars.HasValue)
                .WithMessage("Stars must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .NotEmpty()
                .WithMessage("you must leave a comment")
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Comment))
                .WithMessage("Comment cannot exceed 500 characters.");
        }
    }

}
