using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IUserService
{
    /// <summary>
    /// gets one user
    /// </summary>
    /// <param name="id">id of user</param>
    /// <returns></returns>
    UserDTO GetUserById(int id);
    
    /// <summary>
    /// updates a user
    /// </summary>
    /// <param name="userDto"> userdto holding information to update user</param>
    /// <returns>updated user</returns>
    UserDTO UpdateUser(UserDTO userDto);

    
    /// <summary>
    /// gets all users
    /// </summary>
    List<UserDTO> GetAllUsers();
    /// <summary>
    /// gets all admins
    /// </summary>
    List<UserDTO> GetAllAdmins();
    
    /// <summary>
    /// deletes a user from database
    /// </summary>
    /// <param name="userId">id of user for deletion</param>
    /// <returns>the deleted user</returns>
    User DeleteUser(int userId);
}