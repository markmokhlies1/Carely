using FluentValidation;

namespace Carely.Dtos.Requests.Lullaby.Validation
{
    public class CreateLullabyRequestValidator : AbstractValidator<CreateLullabyRequest>
    {
        public CreateLullabyRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Duration)
                .NotEmpty().WithMessage("Duration is required.")
                .GreaterThan(TimeSpan.Zero).WithMessage("Duration must be greater than zero.");

            //RuleFor(x => x.MotherId)
            //    .GreaterThan(0).WithMessage("MotherId must be valid.");

            RuleFor(x => x.AudioFile).NotNull().WithMessage("Audio file is required.")
                .Must(file => file.Length > 0).WithMessage("Audio file cannot be empty.");
        }
    }
}
