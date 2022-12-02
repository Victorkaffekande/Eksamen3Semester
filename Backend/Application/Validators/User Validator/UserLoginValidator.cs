using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class UserLoginValidator : AbstractValidator<UserLoginDTO>
{
    public UserLoginValidator()
    { 
        RuleFor(u => u.Username).NotNull().NotEmpty();
        RuleFor(u => u.Password).NotNull().NotEmpty();
        
    }
}