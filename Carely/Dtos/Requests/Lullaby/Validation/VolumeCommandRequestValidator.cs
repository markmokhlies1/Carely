using FluentValidation;

namespace Carely.Dtos.Requests.Lullaby.Validation
{
    public class VolumeCommandRequestValidator : AbstractValidator<VolumeCommandRequest>
    {
        public VolumeCommandRequestValidator()
        {
            RuleFor(x => x.LullabyId)
                .GreaterThan(0)
                .WithMessage("LullabyId must be a valid positive number.");

            RuleFor(x => x.Level)
               .InclusiveBetween(0, 100)
               .WithMessage("Volume level must be between 0 and 100.");


        }

    }
}
