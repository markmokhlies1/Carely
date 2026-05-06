using Carely.Dtos.Requests.Medication;
using FluentValidation;

namespace Carely.Dtos.Requests.Baby.Validation
{
    public class UpdateBabyRequestValidator : AbstractValidator<UpdateBabyRequest>
    {
        public UpdateBabyRequestValidator()
        {
            RuleFor(x => x.FirstName)
               .NotEmpty().WithMessage("FirstName is required")
               .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("LastName is Required")
                .MaximumLength(50);

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Invalid gender value");

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Weight must be greater than 0")
                .LessThanOrEqualTo(20).WithMessage("Weight must not exceed 20 kg");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Date of Birth cannot be in the future")
                .GreaterThan(DateTime.Today.AddYears(-3)).WithMessage("Age cannot exceed 3 years.");

            RuleFor(x => x.Developmental)
                .IsInEnum()
                .WithMessage("Developmental status must be a valid option.")
                .NotNull().WithMessage("Developmental status is required.");


        }
    }
}
