using FluentValidation;

namespace Carely.Dtos.Requests.Doctor.Validation
{
    public class CreateDoctorRequestValidator : AbstractValidator<CreateDoctorRequest>
    {
        public CreateDoctorRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50);

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
                .Matches(@"^\+?\d{8,15}$")
                .WithMessage("Phone number must be valid.");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Invalid gender value.");

            RuleFor(x => x.Age)
                .GreaterThanOrEqualTo(18).WithMessage("Minimum age is 18.");

            RuleFor(x => x.Link)
                .Must(link => string.IsNullOrEmpty(link) || Uri.IsWellFormedUriString(link, UriKind.Absolute))
                .WithMessage("Link must be a valid URL.");
        }
    }

}
