using Application.Interfaces;
using Domain;

namespace Infrastructure;

public class UserRepository : IUserRepository
{
    private DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public User GetUserById(int id)
    {
        return _context.UserTable.Find(id);
    }

    public User UpdateUser(User user)
    {
        var userById = GetUserById(user.Id);
        if (userById.Id.Equals(user.Id))
        {
            userById.Username = user.Username;
            userById.Email = user.Email;
            userById.BirthDay = user.BirthDay;
            if (user.ProfilePicture != null) userById.ProfilePicture = user.ProfilePicture;
        }

        _context.UserTable.Update(userById ?? throw new InvalidOperationException());
        _context.SaveChanges();
        return userById;
    }
}