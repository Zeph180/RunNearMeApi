using Application.Models.Request.Challenge;
using FluentValidation;

namespace Application.Validators.RequestValidations.Challenge;

public class UpdateChallengeArtRequestValidation : AbstractValidator<UpdateChallengeArtRequest>
{
    public UpdateChallengeArtRequestValidation()
    {
        RuleFor(r => r.ChallengeId)
            .NotEmpty().WithMessage("You must provide a challenge id");
        
        RuleFor(r => r.RunnerId)
            .NotEmpty().WithMessage("You must provide a runner id");

        RuleFor(r => r.ChallengeArt)
            .NotEmpty().WithMessage("You must provide a challenge art");
    }
}