using Application.DTOs;
using Application.Interfaces;

namespace Application;

public class UserService : IUserService
{
    private IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public UserDTO GetUserById(int id)
    {
        if (id < 1) throw new ArgumentException("Id cannot be lower than 1");

        var user = _repo.GetUserById(id);

        if (user == null)
        {
            return null;
        }

        var userDto = new UserDTO()
        {
            Id = user.Id,
            BirthDay = user.BirthDay,
            Email = user.Email,
            ProfilePicture = user.ProfilePicture,
            Username = user.Username
        };

        return userDto;
    }
}