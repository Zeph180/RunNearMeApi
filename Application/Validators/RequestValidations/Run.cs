using Application.Models.Request.Run;
using FluentValidation;

namespace Application.Validators.RequestValidations;

public class Run : AbstractValidator<CreateRunRequest>
{

    public Run()
    {
        RuleFor(x => x.RunnerId)
            .NotNull()
            .WithMessage("Runner Id is required");
    }
}