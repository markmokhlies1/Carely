using Carely.Dtos.Requests.Jwt;
using FluentValidation;

namespace Carely.Dtos.Requests.Jwt.Validation
{
    public class GenerateTokenRequestValidator : AbstractValidator<GenerateTokenRequest>
    {
        public GenerateTokenRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Roles)
                .NotNull().WithMessage("Roles list cannot be null.")
                .Must(r => r.Count > 0).WithMessage("At least one role must be assigned.");

            RuleForEach(x => x.Roles)
                .NotEmpty().WithMessage("Role name cannot be empty.");

            RuleFor(x => x.Permissions)
                .NotNull().WithMessage("Permissions list cannot be null.");
        }
    }

}
