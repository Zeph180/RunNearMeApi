using Application.Models.Request.Authentication;
using FluentValidation;

namespace Application.Validators.RequestValidations.Authentication;

public class CompleteProfileReqValidation : AbstractValidator<CompleteProfileReq>
{
    public CompleteProfileReqValidation()
    {
        RuleFor(x => x.RunnerId) 
            .NotEmpty()
            .WithMessage("RunnerId is required");
        
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .Length(2, 50)
            .WithMessage("Address must be between 2 and 50 characters");

        RuleFor(x => x.Age)
            .NotEmpty()
            .WithMessage("Age can not be empty")
            .GreaterThan(12)
            .WithMessage("Age must be between 12 and 100 characters");
        
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required")
            .Length(2, 50)
            .WithMessage("City must be between 2 and 50 characters");

        RuleFor(x => x.Height)
            .NotEmpty()
            .WithMessage("Height is required")
            .GreaterThan(0)
            .WithMessage("Height must be greater than 0");

        RuleFor(x => x.NickName)
            .NotEmpty()
            .WithMessage("Nickname is required")
            .Length(2, 20)
            .WithMessage("Nickname must be between 2 and 20 characters");

        RuleFor(x => x.PhoneNumber)
            .Length(10, 12)
            .WithMessage("Phone number should be at least 10 digits long")
            .Matches(@"^\d+$")
            .WithMessage("Phone number should contain only digits");

        RuleFor(x => x.State)
            .Length(2, 5)
            .WithMessage("State must be at least 2 characters long");
    }
}