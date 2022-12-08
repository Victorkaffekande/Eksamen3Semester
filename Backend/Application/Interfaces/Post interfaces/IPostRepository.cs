using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IPostRepository
{
    public Post CreatePost(Post post);
    public Post UpdatePost(Post post);
    public Post DeletePost(int id);
    public Post GetPostGetById(int id);
    public List<Post> GetAllPostFromProject(int id);
    
}