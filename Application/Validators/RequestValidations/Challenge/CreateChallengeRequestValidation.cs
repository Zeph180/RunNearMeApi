using Application.Models.Request.Challenge;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validators.RequestValidations.Challenge;

public class CreateChallengeRequestValidation : AbstractValidator<CreateChallengeRequest>
{
    public CreateChallengeRequestValidation()
    {
        RuleFor(x => x.RunnerId)
            .NotEmpty()
            .WithMessage("RunnerId is required");
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MinimumLength(10)
            .WithMessage("Description must be at least 10 characters")
            .MaximumLength(70)
            .WithMessage("Description must be at most 70 characters");
        RuleFor(x => x.EndsAt)
            .NotEmpty()
            .WithMessage("EndsAt is required")
            .GreaterThan(DateTime.Now)
            .WithMessage("EndsAt must be greater than current date");
        RuleFor(x => x.Target)
            .NotEmpty()
            .WithMessage("Target can not be empty");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name can not be empty")
            .MinimumLength(3)
            .WithMessage("Name must be at least 3 characters")
            .MaximumLength(20)
            .WithMessage("Name must be at most 20 characters");

        RuleFor(x => x.ChallengeArt)
            .NotEmpty()
            .WithMessage("ChallengeArt is required")
            .Must(file => IsImage(file))
            .WithMessage("ChallengeArt must be an image (jpg, png, jpeg)")
            .Must(file => file != null && file.Length <= 5 * 1024 * 1024)
            .WithMessage("ChallengeArt must not exceed 5 MB in size");
    }
    
    private bool IsImage(IFormFile file)
    {
        var permittedExtensions = new[] { "jpg", "png", "jpeg" };
        return permittedExtensions.Any(x => file.FileName.EndsWith(x));
    }
    
    private bool ValidSize(IFormFile file)
    {
        return file.Length < 1000000;
    }
}