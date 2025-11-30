using FluentValidation;

namespace Carely.Dtos.Requests.Meeting.Validation
{
    public class UpdateMeetingRequestValidator : AbstractValidator<UpdateMeetingRequest>
    {
        public UpdateMeetingRequestValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(200);

            RuleFor(x => x.MeetingType)
                .IsInEnum().WithMessage("Invalid meeting type.");

            RuleFor(x => x.Date)
                .GreaterThan(DateTime.Now).WithMessage("Meeting date must be in the future.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200);

            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("DoctorId must be valid.");
        }
    }


}
