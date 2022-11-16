using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IAuthService
{

    /// <summary>
    /// registers a new user in the database
    /// </summary>
    /// <param name="userRegisterDto"></param>
    /// <returns> returns a jwtToken</returns>
    public string Register(UserRegisterDTO userRegisterDto);
    
    /// <summary>
    /// login and 
    /// </summary>
    /// <param name="userLoginDto"></param>
    /// <returns>returns a jwtToken</returns>
    public string Login(UserLoginDTO userLoginDto);

    /// <summary>
    /// generate jwtToken
    /// </summary>
    /// <param name="user"></param>
    /// <returns>jwtToken</returns>
    public string GenerateToken(User user);
}