using Application.Models.Request.Challenge;
using FluentValidation;

namespace Application.Validators.RequestValidations.Challenge;

public class UpdateChallengeRequestValidation : AbstractValidator<UpdateChallengeRequest>
{
    public UpdateChallengeRequestValidation()
    {
        RuleFor(c => c.ChallengeId)
            .NotEmpty().WithMessage("You must provide a challenge id");
        
        RuleFor(c => c.RunnerId)
            .NotEmpty().WithMessage("You must provide a runner id");

        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("You must provide a description");
        
        RuleFor(c => c.EndsAt)
            .NotEmpty().WithMessage("You must provide a ends at")
            .GreaterThan(DateTime.Now)
            .WithMessage("End date must be greater than the current date time");

        RuleFor(c => c.Target)
            .NotEmpty().WithMessage("You must provide a target");
    }
}