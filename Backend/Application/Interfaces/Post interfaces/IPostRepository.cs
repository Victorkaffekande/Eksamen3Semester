using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IPostRepository
{
    //crud functions
    public Post CreatePost(Post post);
    public Post UpdatePost(Post post);
    public Post DeletePost(int id);
    
    //gets a post by id
    public Post GetPostGetById(int id);
    
    //gets all post that belongs to a project
    public List<Post> GetAllPostFromProject(int id);
    
}