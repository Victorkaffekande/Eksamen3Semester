using Application.DTOs;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace Application.Validators;

public class ProjectDTOValidator : AbstractValidator<ProjectDTO>
{
    public ProjectDTOValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User id must be 1 or higher");
        

        RuleFor(p => p.Image)
            .Must( x => x.Contains("data:image/png;base64,") || x.Contains("data:image/jpeg;base64,"))
            .When(p => p.Image != null)
            .WithState( x => throw new ArgumentException("Only PNG and JPG files are allowed"));
        
        RuleFor(p => p.Title)
            .NotEmpty()
            .WithMessage("Title can not be empty or null");
        RuleFor(x => x.StartTime)
            .NotNull()
            .WithMessage("Start date can not be null");
    }
}