using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AuthRepository : IAuthRepository
{

    private DatabaseContext _context;
    public AuthRepository(DatabaseContext context)
    {
        _context = context;
        
    }

    public User GetUserByUsername(string username)
    {
        return _context.UserTable.FirstOrDefault(u => u.Username == username) ?? throw new KeyNotFoundException("No user with that Username: " + username);
    }

    public User CreateNewUser(User user)
    {
        _context.UserTable.Add(user);
        _context.SaveChanges();
        return user;
    }

    public void RebuildDatabase()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }
}