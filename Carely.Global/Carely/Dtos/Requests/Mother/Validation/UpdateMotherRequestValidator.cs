using FluentValidation;

namespace Carely.Dtos.Requests.Mother.Validation
{
    public class UpdateMotherRequestValidator : AbstractValidator<UpdateMotherRequest>
    {
        public UpdateMotherRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number format.");

            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.Today).WithMessage("Birth date must be in the past.")
                .GreaterThan(DateTime.Today.AddYears(-100)).WithMessage("Invalid birth date.");

            RuleFor(x => x.Hight)
                .GreaterThan(0).WithMessage("Height must be greater than 0 cm.")
                .LessThan(300).WithMessage("Height seems unrealistic.");

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Weight must be greater than 0 kg.")
                .LessThan(500).WithMessage("Weight seems unrealistic.");
        }
    }
}
