using FluentValidation;

namespace Carely.Dtos.Requests.ClinicWorkTime.Validation
{
    public class UpdateClinicWorkTimeRequestValidator : AbstractValidator<UpdateClinicWorkTimeRequest>
    {
        public UpdateClinicWorkTimeRequestValidator()
        {
            RuleFor(x => x.Day)
                .IsInEnum()
                .When(x => x.Day.HasValue)
                .WithMessage("Invalid day of the week.");

            RuleFor(x => x.From)
                .LessThan(x => x.To)
                .When(x => x.From.HasValue && x.To.HasValue)
                .WithMessage("Start time must be earlier than end time.");

            RuleFor(x => x.To)
                .GreaterThan(x => x.From)
                .When(x => x.From.HasValue && x.To.HasValue)
                .WithMessage("End time must be later than start time.");
        }
    }

}
