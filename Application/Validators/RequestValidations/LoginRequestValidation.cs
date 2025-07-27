using Domain.Models.Request.Authentication;
using FluentValidation;

namespace Application.Validators.RequestValidations;

public class LoginRequestValidation : AbstractValidator<LoginRequest>
{
    public LoginRequestValidation()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .Length(6, 100)
            .WithMessage("Password must be between 6 and 100 characters");
    }
}