using Application.Models.Request.Challenge;
using FluentValidation;

namespace Application.Validators.RequestValidations.Challenge;

public class GetChallengesRequestValidation : AbstractValidator<GetChallengesRequest>
{
    public GetChallengesRequestValidation()
    {
        RuleFor(r => r.RunnerId)
            .NotEmpty()
            .WithMessage("You must provide a runner id");

        RuleFor(r => r.PageNumber)
            .NotEmpty()
            .WithMessage("You must provide a page number")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Page number must be greater than or equal to 0");
        
        RuleFor(r => r.PageSize)
            .NotEmpty()
            .WithMessage("You must provide a page size")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Page size must be greater than or equal to 0");
    }
}