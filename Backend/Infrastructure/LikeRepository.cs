using Application.DTOs;
using Application.Interfaces.Like_Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class LikeRepository : ILikeRepository
{
    private DatabaseContext _context;

    public LikeRepository(DatabaseContext context)
    {
        _context = context;
    }

    public Like LikeUser(Like like)
    {
         _context.LikeTable.Add(like);
         _context.SaveChanges();
         return like;
    }

    public Like RemoveLike(Like like)
    {
        var l = _context.LikeTable.Find(like.UserId, like.LikedUserId);
        _context.LikeTable.Remove(l);
        _context.SaveChanges();
        return like;
    }

    public Like AlreadyLikes(Like like)
    {
        return _context.LikeTable.Find(like.UserId, like.LikedUserId);
    }

    public List<Like> GetAllLikedUsersByUser(int userId)
    {
       return _context.LikeTable.Include(l => l.LikedUser).Where(l => l.UserId == userId).ToList();
    }

    public List<Like> GetallPostByLikedUsersByUser(int userId)
    {
        return _context.LikeTable.Include(l => l.LikedUser.Projects).ThenInclude(p => p.Posts).Where(l => l.UserId == userId).ToList();
    }
    public List<DashboardPostDTO> GetAllPostByLikedUsers(List<User> users, int start, int end)
    {
        return _context.PostTable.Include(p => p.Project.User).Where(p =>users.Contains(p.Project.User)).IgnoreAutoIncludes().Select(p => new DashboardPostDTO()
        {
            Description = p.Description,
            Id = p.Id,
            Image = p.Image,
            PostDate = p.PostDate,
            ProfilePicture = p.Project.User.ProfilePicture,
            ProjectId = p.Project.Id,
            Title = p.Project.Title,
            UserId = p.Project.UserId,
            Username = p.Project.User.Username
        }).OrderByDescending(p => p.PostDate).Skip(0).Take(25).ToList();
    }
}