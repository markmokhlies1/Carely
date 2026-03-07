using Carely.Dtos.Requests.ClinicWorkTime.Validation;
using FluentValidation;

namespace Carely.Dtos.Requests.Clinic.Validation
{
    public class CreateClinicRequestValidator : AbstractValidator<CreateClinicRequest>
    {
        public CreateClinicRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Clinic name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Address)
                .MaximumLength(200);

            RuleFor(x => x.City)
                .MaximumLength(50);

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?\d{8,15}$")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("Phone number must be valid.");

            RuleForEach(x => x.WorkTimes)
                .SetValidator(new CreateClinicWorkTimeRequestValidator());
        }
    }
}
