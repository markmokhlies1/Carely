using FluentValidation;

namespace Carely.Dtos.Requests.Lullaby.Validation
{
    public class PlayCommandRequestValidator : AbstractValidator<PlayCommandRequest>
    {
        public PlayCommandRequestValidator()
        {
          

            RuleFor(x => x.LullabyId)
                .GreaterThan(0)
                .WithMessage("LullabyId must be a valid positive number.");

            
        }
    }
}
