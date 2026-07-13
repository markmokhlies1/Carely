using FluentValidation;
namespace Carely.Dtos.Requests.Mother.Validation
{
    public class UpdateDeviceTokenRequestValidator : AbstractValidator<UpdateDeviceTokenRequest>
    {
        public UpdateDeviceTokenRequestValidator()
        {
            RuleFor(x => x.DeviceToken)
                .NotEmpty()
                .WithMessage("Device token is required.")
                .NotNull()
                .WithMessage("Device token cannot be null.");
        }
    }
}