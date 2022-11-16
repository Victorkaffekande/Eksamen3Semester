using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTOs;
using Application.Interfaces;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace Application;

public class AuthService : IAuthService
{
    private string fillerScret = "asdasdasd";
    private IAuthRepository _repo;
    public AuthService(IAuthRepository repo)    
    {
        _repo = repo;
    }

    public string Register(UserRegisterDTO dto)
    {
        try
        {
            _repo.GetUserByUsername(dto.Username);
        }
        catch (KeyNotFoundException)
        {
            var salt = RandomNumberGenerator.GetBytes(32).ToString();
            var user = new User() //TODO Validate user and get rest of the properties
            {
                Username = dto.Username,
                Salt = salt,
                PwHashed = BCrypt.Net.BCrypt.HashPassword(dto.Password + salt),
                Role = dto.Role
            };
            _repo.CreateNewUser(user);
            return GenerateToken(user);
        }
        throw new Exception("Username" + dto.Username + " Is already taken");
    }

    public string Login(UserLoginDTO dto)
    {
        var user = _repo.GetUserByUsername(dto.Username);
        if (BCrypt.Net.BCrypt.Verify(dto.Password + user.Salt, user.PwHashed))
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
}