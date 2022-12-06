using Domain;

namespace Application.Interfaces;

public interface IUserRepository
{ 
    User GetUserById(int id);

    User UpdateUser(User user);

    List<User> GetAllUsers();
}