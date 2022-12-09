using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IUserService
{
    UserDTO GetUserById(int id);
    UserDTO UpdateUser(UserDTO userDto);

    List<UserDTO> GetAllUsers();
    List<UserDTO> GetAllAdmins();
    User DeleteUser(int userId);
}