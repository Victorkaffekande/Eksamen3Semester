using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class UserDTOValidator : AbstractValidator<UserDTO>
{
    public UserDTOValidator()
    {
        RuleFor(u => u.Id).GreaterThan(0).WithState(x =>throw new ArgumentException("id cannot be below 1"));
        RuleFor(u => u.Username).NotNull().NotEmpty();
        RuleFor(u => u.BirthDay).Must(BeValidDate);
        
        RuleFor(u => u.Email).EmailAddress();
    }
    
    private bool BeValidDate(DateOnly dateOnly)
    {
        return !dateOnly.Equals(default(DateOnly));
    }
}