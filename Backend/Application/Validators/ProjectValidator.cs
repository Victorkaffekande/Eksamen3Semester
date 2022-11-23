using Application.DTOs;
using Domain;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace Application.Validators;

public class ProjectValidator : AbstractValidator<Project>
{
    public ProjectValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Project id must be 1 or higher");
        
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

        RuleFor(x => x.PatternId)
            .GreaterThan(0)
            .When(x => x.Pattern != null)
            .WithState(x => throw new ArgumentException("Project must have a pattern Id, if a pattern is connected"));


    }
}