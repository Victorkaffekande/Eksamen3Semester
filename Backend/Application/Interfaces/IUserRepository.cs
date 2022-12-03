using Domain;

namespace Application.Interfaces;

public interface IUserRepository
{ 
    User GetUserById(int id);
}