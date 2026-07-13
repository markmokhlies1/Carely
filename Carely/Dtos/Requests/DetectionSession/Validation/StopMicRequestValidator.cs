using Carely.Dtos.Requests.Lullaby;
using FluentValidation;

namespace Carely.Dtos.Requests.DetectionSession.Validation
{
    public class StopMicRequestValidator : AbstractValidator<StopMicRequest>
    {
        public StopMicRequestValidator()
        {
            RuleFor(x => x.BabyId)
              .GreaterThan(0)
              .WithMessage("BabyId must be a valid positive number.");
        }
        }
}
