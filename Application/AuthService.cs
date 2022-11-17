using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace Application;

public class AuthService : IAuthService
{
    private string fillerScret = "asdasdasd";
    private IAuthRepository _repo;
    private IValidator<UserRegisterDTO> _registerValidator;
    private IValidator<UserLoginDTO> _loginValidator;
    private IMapper _mapper;
    public AuthService(IAuthRepository repo, 
        IValidator<UserRegisterDTO> registerValidator, 
        IValidator<UserLoginDTO> loginValidator,
        IMapper mapper
        )
    {
        _mapper = mapper;
        _repo = repo;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        
    }

    public string Register(UserRegisterDTO dto)
    {

        var val = _registerValidator.Validate(dto);
        if (!val.IsValid) throw new ValidationException(val.ToString());
            
        try
        {
            _repo.GetUserByUsername(dto.Username);
        }
        catch (KeyNotFoundException)
        {
            var salt = RandomNumberGenerator.GetBytes(32).ToString();

            var user = _mapper.Map<User>(dto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password + salt);
            
            _repo.CreateNewUser(user);
            return GenerateToken(user);
        }
        throw new Exception("Username" + dto.Username + " Is already taken");
    }

    public string Login(UserLoginDTO dto)
    {
        
        var val = _loginValidator.Validate(dto);
        if (!val.IsValid) throw new ValidationException(val.ToString());

        
        var user = _repo.GetUserByUsername(dto.Username);
        if (BCrypt.Net.BCrypt.Verify(dto.Password + user.Salt, user.Password))
        {
            return GenerateToken(user);
        }
        throw new Exception("Invalid Login");
    }

    public string GenerateToken(User user)
    {
        var key =Encoding.UTF8.GetBytes(fillerScret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("username", user.Username),new Claim("role", user.Role) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }

    public void RebuildDatabase()
    {
        _repo.RebuildDatabase();
    }
}