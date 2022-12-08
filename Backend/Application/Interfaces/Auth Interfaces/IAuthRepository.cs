 using Domain;

namespace Application.Interfaces;

public interface IAuthRepository
{
    public User GetUserByUsername(string username);

    public User CreateNewUser(User user);

    public void RebuildDatabase();
}