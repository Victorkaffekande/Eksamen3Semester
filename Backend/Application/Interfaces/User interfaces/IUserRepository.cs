using Domain;

namespace Application.Interfaces;

public interface IUserRepository
{ 
    
    /// <summary>
    /// gets a user
    /// </summary>
    /// <param name="id">if of wanted user</param>
    /// <returns>user</returns>
    User GetUserById(int id);
    
    /// <summary>
    /// updates a user
    /// </summary>
    /// <param name="user"> the user for update</param>
    /// <returns>updated user</returns>
    User UpdateUser(User user);

    /// <summary>
    /// gets all users
    /// </summary>
    List<User> GetAllUsers();
    
    /// <summary>
    /// gets all admins
    /// </summary>
    List<User> GetAllAdmins();

    /// <summary>
    /// deletes a user
    /// </summary>
    /// <param name="userId"> users id</param>
    /// <returns>deleted user</returns>
    User DeleteUser(int userId);

}