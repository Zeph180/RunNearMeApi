using Application.Models.Request.People;
using FluentValidation;

namespace Application.Validators.RequestValidations.People;

public class GetPersonRequestValidation : AbstractValidator<GetPersonRequest>
{
    public GetPersonRequestValidation()
    {
        RuleFor(x => x.RunnerId)
            .NotEmpty()
            .WithMessage("RunnerId is required");
        RuleFor(x => x.PersonId)
            .NotEmpty()
            .WithMessage("PersonId is required");
    }
}