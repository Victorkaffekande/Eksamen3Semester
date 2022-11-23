using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IPostService
{
    public Post CreatePost(PostCreateDTO dto);
    public Post UpdatePost(PostUpdateDTO dto);
    public Post DeletePost(int id);
    public Post GetPostById(int id);
    public List<Post> GetAllPosts();
    public List<Post> GetAllPostFromProject(int id);
}