using FluentValidation;

namespace Carely.Dtos.Requests.Medication.Validation
{
    public class UpdateMedicationRequestValidator : AbstractValidator<UpdateMedicationRequest>
    {
        public UpdateMedicationRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.Spot)
                .NotNull().WithMessage("Spot is required.");

            RuleFor(x => x.StartDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Start date cannot be in the past.");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Duration must be greater than zero.");

            RuleFor(x => x.MedicationType)
                .IsInEnum().WithMessage("Invalid medication type.");
        }
    }
}
