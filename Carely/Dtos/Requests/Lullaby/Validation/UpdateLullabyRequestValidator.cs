using FluentValidation;

namespace Carely.Dtos.Requests.Lullaby.Validation
{
    public class UpdateLullabyRequestValidator : AbstractValidator<UpdateLullabyRequest>
    {
        public UpdateLullabyRequestValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .MaximumLength(100)
            .When(x => x.Name != null);

            RuleFor(x => x.Duration)
                .Must(d => d.HasValue && d.Value > TimeSpan.Zero)
                .WithMessage("Duration must be greater than zero.")
                .When(x => x.Duration.HasValue);

            RuleFor(x => x.AudioFile)
                .Must(file => file != null && file.Length > 0)
                .WithMessage("Audio file cannot be empty.")
                .When(x => x.AudioFile != null);





        }
    }
}
