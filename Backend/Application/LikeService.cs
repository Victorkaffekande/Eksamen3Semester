using Application.DTOs;
using Application.DTOs.Like;
using Application.Interfaces.Like_Interfaces;
using AutoMapper;
using Domain;

namespace Application;

public class LikeService : ILikeService
{
    private ILikeRepository _repo;
    private IMapper _mapper;

    public LikeService(ILikeRepository repo, IMapper mapper)
    {
        if (repo == null) throw new ArgumentException("Missing repository");
        if (mapper == null) throw new ArgumentException("Missing mapper");
        _repo = repo;
        _mapper = mapper;
    }

    public SimpleLikeDto LikeUser(SimpleLikeDto dto)
    {
        if (dto == null) throw new ArgumentException("SimpleLikeDto is null");
        if (AlreadyLike(dto)) throw new ArgumentException("Like already exists");
        if (_repo.DoesUserExist(dto)) throw new ArgumentException("One or more userIds don't exist");


        var like = _mapper.Map<Like>(dto);
        return _mapper.Map<SimpleLikeDto>(_repo.LikeUser(like));
    }


    public SimpleLikeDto RemoveLike(SimpleLikeDto dto)
    {
        if (dto == null) throw new ArgumentException("Input is null");
        var like = _mapper.Map<Like>(dto);
        return _mapper.Map<SimpleLikeDto>(_repo.RemoveLike(like));
    }

    public bool AlreadyLike(SimpleLikeDto dto)
    {
        var l = _repo.AlreadyLikes(_mapper.Map<Like>(dto));
        if (l != null)
        {
            return true;
        }

        return false;
    }

    public List<UserDTO> GetAllLikedUsersByUser(int userId)
    {
        return _mapper.Map<List<UserDTO>>(_repo.GetAllLikedUsersByUser(userId));
    }


    public List<DashboardPostDTO> GetallPostByLikedUsersByUser(int userId)
    {
        var dtoList = new List<DashboardPostDTO>();
        foreach (var l in _repo.GetallPostByLikedUsersByUser(userId))
        {
            foreach (var p in l.LikedUser.Projects)
            {
                foreach (var post in p.Posts)
                {
                    var dto = new DashboardPostDTO()
                    {
                        Description = post.Description,
                        Id = post.Id,
                        Image = post.Image,
                        PostDate = post.PostDate,
                        ProfilePicture = l.LikedUser.ProfilePicture,
                        ProjectId = p.Id,
                        Title = p.Title,
                        UserId = l.LikedUserId,
                        Username = l.LikedUser.Username
                    };
                    dtoList.Add(dto);
                }
            }
        }

        return dtoList.OrderBy(p => p.PostDate).ToList();
    }
}