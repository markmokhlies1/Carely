using FluentValidation;

namespace Carely.Dtos.Requests.Lullaby.Validation
{
    public class StopCommandRequestValidator : AbstractValidator<StopCommandRequest>
    {
        public StopCommandRequestValidator()
        {
          

            RuleFor(x => x.LullabyId)
                .GreaterThan(0)
                .WithMessage("LullabyId must be a valid positive number.");

            //RuleFor(x => x.StopPosition)
            //    .Must(sp => sp >= TimeSpan.Zero)
            //    .WithMessage("StopPosition cannot be negative.")
            //    .Must(sp => sp < TimeSpan.FromHours(24))
            //    .WithMessage("StopPosition must be less than 24 hours.");

        }
    }
}
