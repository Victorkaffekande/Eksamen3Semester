using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;

namespace Application;

public class PostService : IPostService
{

    private IPostRepository _repo;
    private IMapper _mapper;
    private PostCreateDTOValidator _postCreateDtoValidator;
    private PostUpdateValidator _postUpdateValidator;
    
    public PostService(IPostRepository repo,
        IMapper mapper,
        PostCreateDTOValidator createDtoValidator,
        PostUpdateValidator postUpdateValidator
    )
    {
        _repo = repo ?? throw new ArgumentException("Missing Repository"); 
        _mapper = mapper ?? throw new ArgumentException("Missing Mapper");
        _postCreateDtoValidator = createDtoValidator ?? throw new ArgumentException("Missing Validator");
        _postUpdateValidator = postUpdateValidator ?? throw new ArgumentException("Missing Validator");
    }

    public Post CreatePost(PostCreateDTO dto)
    {
        if (dto is null) throw new ArgumentException("PostDTO is null");
        

        var val = _postCreateDtoValidator.Validate(dto);
        if (!val.IsValid) throw new ArgumentException(val.ToString());


        return _repo.CreatePost(_mapper.Map<Post>(dto));    }

    public Post UpdatePost(PostUpdateDTO dto)
    {
        if (dto is null) throw new ArgumentException("Post is null");
        Post post = _mapper.Map<Post>(dto);
        
        var val = _postUpdateValidator.Validate(post);
        if (!val.IsValid) throw new ArgumentException(val.ToString());

        
        if (_repo.GetPostGetById(post.Id) == null) throw new ArgumentException("Post id does not exist");

        return _repo.UpdatePost(post);
    }

    public Post DeletePost(int id)
    {
        if (id < 1) throw new ArgumentException("id cannot be under 1");

        
        if (GetPostById(id) == null) throw new ArgumentException("Post does not exist");
        
        return _repo.DeletePost(id);
    }

    public Post GetPostById(int id)
    {
        if (id<1) throw new ArgumentException("Id cannot be lower than 1");

        return _repo.GetPostGetById(id);
    }

    public List<Post> GetAllPosts()
    {
        return _repo.GetAllPosts();
        
    }

    public List<Post> GetAllPostFromProject(int id)
    {
        if (id<1) throw new ArgumentException("Id cannot be lower than 1");
        
       return _repo.GetAllPostFromProject(id);
    }
}