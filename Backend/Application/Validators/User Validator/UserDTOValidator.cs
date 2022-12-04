using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class UserDTOValidator : AbstractValidator<UserDTO>
{
    public UserDTOValidator()
    {
        RuleFor(u => u.Id).GreaterThan(0).WithState(x =>throw new ArgumentException("id cannot be below 1"));
        RuleFor(u => u.Username).NotNull().NotEmpty().WithState(x => throw new ArgumentException("Username can not be empty or null"));
        RuleFor(u => u.BirthDay).Must(BeValidDate).WithState(x => throw new ArgumentException("Birthday has to be a date"));
        
        RuleFor(u => u.Email).NotEmpty().WithState(x => throw new ArgumentException("Email has to be an Email")).EmailAddress().WithState(x => throw new ArgumentException("Email has to be an Email"));
        
        RuleFor(u => u.ProfilePicture)
            .Must( x => x.Contains("data:image/png;base64,") || x.Contains("data:image/jpeg;base64,"))
            .When(u => u.ProfilePicture != null)
            .WithState( x => throw new ArgumentException("Only PNG and JPG files are allowed"));

    }
    
    private bool BeValidDate(DateOnly dateOnly)
    {
        return !dateOnly.Equals(default(DateOnly));
    }
}