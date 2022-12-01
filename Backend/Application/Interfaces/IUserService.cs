using Application.DTOs;

namespace Application.Interfaces;

public interface IUserService
{
    UserDTO GetUserById(int id);
}