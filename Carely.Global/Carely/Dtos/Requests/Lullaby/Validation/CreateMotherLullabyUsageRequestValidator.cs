using FluentValidation;

namespace Carely.Dtos.Requests.Lullaby.Validation
{
    public class CreateMotherLullabyUsageRequestValidator : AbstractValidator<CreateMotherLullabyUsageRequest>
    {
        public CreateMotherLullabyUsageRequestValidator()
        {
            RuleFor(x => x.MotherId)
                .GreaterThan(0)
                .WithMessage("MotherId must be a valid positive number.");

            RuleFor(x => x.LullabyId)
                .GreaterThan(0)
                .WithMessage("LullabyId must be a valid positive number.");

            RuleFor(x => x.PlayCount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("PlayCount cannot be negative.");
        }
    }
}
