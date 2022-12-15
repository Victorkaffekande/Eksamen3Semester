using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class PostRepository : IPostRepository
{
    private DatabaseContext _context;

    public PostRepository( DatabaseContext context)
    {
        _context = context;
    }

    public Post CreatePost(Post post)
    {
        _context.PostTable.Add(post);
        _context.SaveChanges();
        return post;
    }

    public Post UpdatePost(Post post)
    {
        var oldPost = GetPostGetById(post.Id);
        if (oldPost.Id.Equals(post.Id))
        {
            oldPost.Description = post.Description;
            oldPost.Image = post.Image;
        }

        _context.PostTable.Update(oldPost ?? throw new InvalidOperationException());
        _context.SaveChanges();
        return oldPost;
    }

    public Post DeletePost(int id)
    {
        var post = _context.PostTable.Find(id);
        _context.PostTable.Remove(post ?? throw new InvalidOperationException());
        _context.SaveChanges();
        return post;    
    }

    public Post GetPostGetById(int id)
    {
        return _context.PostTable.Find(id);    }
    public List<Post> GetAllPostFromProject(int id)
    {
        return _context.PostTable.Where(p => p.ProjectId == id).ToList();
    }

}