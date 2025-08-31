using Application.Models.Request.Challenge;
using FluentValidation;

namespace Application.Validators.RequestValidations.Challenge;

public class ChallengeJoinRequestValidation : AbstractValidator<ChallengeJoinRequest>
{
    public ChallengeJoinRequestValidation()
    {
        RuleFor(c => c.ChallengeId)
            .NotEmpty()
            .WithMessage("You must provide a challenge id");
        
        RuleFor(c  => c.RunnerId)
            .NotEmpty()
            .WithMessage("You must provide a runner id");
    }
}