using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IPostService
{
    //crud functions
    public PostFromProjectDTO CreatePost(PostCreateDTO dto);
    public Post UpdatePost(PostUpdateDTO dto);
    public Post DeletePost(int id);
    
    //get post by an id
    public Post GetPostById(int id);
    
    //get all post that belongs to a project (returns PostFromProjectDto to limit data transfer)
    public List<PostFromProjectDTO> GetAllPostFromProject(int id);
}