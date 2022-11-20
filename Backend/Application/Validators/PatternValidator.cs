using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class PatternValidator : AbstractValidator<PatternDTO>
{
    public PatternValidator()
    {
        RuleFor(p => p.Image).NotNull().WithState(x => new ArgumentException("No image selected"));
        RuleFor(p => p.PdfString).NotNull().WithState(x => new ArgumentException("No pdf selected"));
        RuleFor(p => p.Description).NotNull().WithState(x => new ArgumentException("Description is missing"));
        RuleFor(p => p.User).NotNull().WithState(x => new ArgumentException("No User"));
        RuleFor(p => p.UserId).NotNull().WithState(x => new ArgumentException("No User id"));
    }
}