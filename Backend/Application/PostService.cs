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

    public PostFromProjectDTO CreatePost(PostCreateDTO dto)
    {
        if (dto is null) throw new ArgumentException("PostDTO is null");


        var val = _postCreateDtoValidator.Validate(dto);
        if (!val.IsValid) throw new ArgumentException(val.ToString());

//manual map
        var response = _repo.CreatePost(_mapper.Map<Post>(dto));
        var returnDto = new PostFromProjectDTO()
        {
            Id = response.Id,
            Description = response.Description,
            Image = response.Image,
            PostDate = response.PostDate,
        };
        return returnDto;
    }

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
        if (id < 1) throw new ArgumentException("Id cannot be lower than 1");

        return _repo.GetPostGetById(id);
    }

    public List<PostGetAllDTO> GetAllPosts()
    {
        List<PostGetAllDTO> allPost = new List<PostGetAllDTO>();
        foreach (var p in _repo.GetAllPosts())
        {
            var post = new PostGetAllDTO()
            {
                Id = p.Id,
                Description = p.Description,
                Image = p.Image,
                PostDate = p.PostDate,
                Project = new ProjectGetAllDTO()
                {
                    Id = p.Project.Id,
                    Image = null,
                    Title = p.Project.Title,
                    User = new UserDTO()
                    {
                        BirthDay = p.Project.User.BirthDay,
                        Email = p.Project.User.Email,
                        Id = p.Project.User.Id,
                        ProfilePicture = p.Project.User.ProfilePicture,
                        Username = p.Project.User.Username
                    }
                }
            };
            allPost.Add(post);
        }

        return allPost;
    }

    public List<PostFromProjectDTO> GetAllPostFromProject(int id)
    {
        if (id < 1) throw new ArgumentException("Id cannot be lower than 1");

        var allPost = new List<PostFromProjectDTO>();
        foreach (var p in _repo.GetAllPostFromProject(id))
        {
            var post = new PostFromProjectDTO()
            {
                Id = p.Id,
                Description = p.Description,
                Image = p.Image,
                PostDate = p.PostDate,
            };
            allPost.Add(post);
        }

        return allPost;
    }
}