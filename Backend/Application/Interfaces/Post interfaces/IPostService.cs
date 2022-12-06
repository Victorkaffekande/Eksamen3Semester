using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IPostService
{
    public PostFromProjectDTO CreatePost(PostCreateDTO dto);
    public Post UpdatePost(PostUpdateDTO dto);
    public Post DeletePost(int id);
    public Post GetPostById(int id);
    public List<DashboardPostDTO> GetAllPosts();
    public List<PostFromProjectDTO> GetAllPostFromProject(int id);
}