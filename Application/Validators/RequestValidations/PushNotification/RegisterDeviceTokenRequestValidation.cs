using Application.Interfaces.Dtos.PushNotifications;
using FluentValidation;

namespace Application.Validators.RequestValidations.PushNotification;

public class RegisterDeviceTokenRequestValidation : AbstractValidator<RegisterDeviceTokenRequest>
{
    public RegisterDeviceTokenRequestValidation()
    {
        RuleFor(x => x.Platform)
            .NotEmpty()
            .WithMessage("Platform is required")
            .Must(p => p.Equals("IOS") || p.Equals("ANDROID"))
            .WithMessage("Platform must be IOS or ANDROID");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required")
            .Must(id => id != Guid.Empty)
            .WithMessage("User ID cannot be empty GUID");
        
        RuleFor(x => x.DeviceToken)
            .NotEmpty()
            .WithMessage("Device token is required")
            .MinimumLength(140)
            .WithMessage("Device token length is too short to be valid")
            .MaximumLength(200)
            .WithMessage("Device token length is too long to be valid");
    }
}