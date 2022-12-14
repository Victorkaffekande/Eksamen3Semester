using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class PostCreateDTOValidator : AbstractValidator<PostCreateDTO>
{
    public PostCreateDTOValidator()
    {
        RuleFor(x => x.ProjectId)
            .GreaterThan(0)
            .WithMessage("Project id must be 1 or higher");

        RuleFor(p => p.Image).NotNull().WithState(x => throw new ArgumentException("Image can not be null"));

        RuleFor(p => p.Image)
            .Must( x => x.Contains("data:image/png;base64,") || x.Contains("data:image/jpeg;base64,"))
            .When(p => p.Image != null)
            .WithState( x => throw new ArgumentException("Only PNG and JPG files are allowed"));
        
        RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage("Description can not be empty or null");
        RuleFor(x => x.PostDate)
            .NotNull()
            .WithMessage("Post date can not be null");

        RuleFor(x => x.ProjectId)
            .GreaterThan(0)
            
            .WithState(x => throw new ArgumentException("Post must have a pattern Id"));
    }
}