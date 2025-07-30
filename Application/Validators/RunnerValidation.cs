using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using FluentValidation;

namespace Application.Validators;

public class RunnerValidation : AbstractValidator<Runner>
{
    public RunnerValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .Length(2, 50)
            .WithMessage("Name must be between 2 and 50 characters");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .Length(2, 50)
            .WithMessage("Email must be between 2 and 50 characters")
            .EmailAddress()
            .WithMessage("Email must be a valid email address");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .Length(6, 10)
            .WithMessage("Password must have between 6 and 10 characters");
    }
}