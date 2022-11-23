using Application.DTOs;
using Domain;
using FluentValidation;

namespace Application.Validators;

public class PatternValidator : AbstractValidator<Pattern>
{
    public PatternValidator()
    {
        
        
        RuleFor(p => p.UserId).GreaterThan(0).WithState(x => throw new ArgumentException("UserID must be higher than 0"));
        RuleFor(p => p.Id).GreaterThan(0).WithState(x => throw new ArgumentException("ID must be higher than 0"));
        

        RuleFor(p => p.Description).NotEmpty().WithState(x => throw new ArgumentException("Description can not be empty or null"));
        RuleFor(p => p.Title).NotEmpty().WithState(x => throw new ArgumentException("title can not be empty or null"));

        RuleFor(p => p.PdfString).NotNull().WithState(x => throw new ArgumentException("this is a not pdf"));
        RuleFor(p => p.Image).NotNull().WithState(x => throw new ArgumentException("this is not a png/jpeg"));

        
        RuleFor(p => p.PdfString).Matches("data:application/pdf;base64,").WithState(x => throw new ArgumentException("this is a not pdf"));
        RuleFor(p => p.Image).Must(x => x.Contains("data:image/png;base64,") || x.Contains("data:image/jpeg;base64,")).WithState(x => throw new ArgumentException("this is not a png/jpeg"));
    }
    
}