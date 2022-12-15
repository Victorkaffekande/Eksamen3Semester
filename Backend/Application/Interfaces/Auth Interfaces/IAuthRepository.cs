 using Domain;

namespace Application.Interfaces;

public interface IAuthRepository
{
    /// <summary>
    /// finds a user from username
    /// </summary>
    /// <param name="username">username of username</param>
    /// <returns>returns the full user</returns>
    public User GetUserByUsername(string username);

    
    /// <summary>
    /// creates a new user
    /// </summary>
    /// <param name="user">user object for creation</param>
    /// <returns>the created user</returns>
    public User CreateNewUser(User user);

    /// <summary>
    /// rebuilds the database.
    /// </summary>
    public void RebuildDatabase();
}