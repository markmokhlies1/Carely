using FluentValidation;

namespace Carely.Dtos.Requests.ClinicWorkTime.Validation
{
    public class CreateClinicWorkTimeRequestValidator : AbstractValidator<CreateClinicWorkTimeRequest>
    {
        public CreateClinicWorkTimeRequestValidator()
        {
            RuleFor(x => x.Day)
                .IsInEnum()
                .WithMessage("Invalid day of the week.");

            RuleFor(x => x.From)
                .LessThan(x => x.To)
                .WithMessage("Start time must be earlier than end time.");

            RuleFor(x => x.To)
                .GreaterThan(x => x.From)
                .WithMessage("End time must be later than start time.");
        }
    }

}
