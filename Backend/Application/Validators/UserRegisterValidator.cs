using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class UserRegisterValidator : AbstractValidator<UserRegisterDTO>
{
    public UserRegisterValidator()
    {
        
        RuleFor(u => u.Username).NotNull().NotEmpty();
        RuleFor(u => u.Birthday).Must(BeValidDate);
        
        RuleFor(u => u.Email).EmailAddress();
        
        RuleFor(u => u.Password).NotNull().NotEmpty();
        //RuleFor(u => u.Password.Length).GreaterThan(4);

    }

    private bool BeValidDate(DateOnly dateOnly)
    {
        return !dateOnly.Equals(default(DateOnly));
    }
}