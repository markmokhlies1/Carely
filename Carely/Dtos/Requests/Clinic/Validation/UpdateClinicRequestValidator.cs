using Carely.Dtos.Requests.ClinicWorkTime.Validation;
using FluentValidation;

namespace Carely.Dtos.Requests.Clinic.Validation
{
    public class UpdateClinicRequestValidator : AbstractValidator<UpdateClinicRequest>
    {
        public UpdateClinicRequestValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100);

            RuleFor(x => x.Address)
                .MaximumLength(200);

            RuleFor(x => x.City)
                .MaximumLength(50);

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?\d{8,15}$")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("Phone number must be valid.");

            When(x => x.WorkTimes != null, () =>
            {
                RuleForEach(x => x.WorkTimes)
                    .SetValidator(new UpdateClinicWorkTimeRequestValidator());
            });
        }
    }
}
