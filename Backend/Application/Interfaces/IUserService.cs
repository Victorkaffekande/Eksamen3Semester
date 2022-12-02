using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IUserService
{
    UserDTO GetUserById(int id);
    User UpdateUser(UserDTO userDto);
}