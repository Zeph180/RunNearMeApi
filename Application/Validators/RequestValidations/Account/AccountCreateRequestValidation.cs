using System.Text.RegularExpressions;
using Domain.Models.Request.Account;
using FluentValidation;

namespace Application.Validators.RequestValidations.Account;

public class AccountCreateRequestValidation : AbstractValidator<AccountCreateRequest>
{
    public AccountCreateRequestValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 50).WithMessage("Name must be between 2 and 50 characters");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .Length(6, 100).WithMessage("Password must be between 6 and 100 characters");
    }
}