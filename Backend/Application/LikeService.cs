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
        if (!_repo.DoesUserExist(dto)) throw new ArgumentException("One or more userIds don't exist");


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
        return _mapper.Map<List<UserDTO>>(_repo.GetAllLikedUsersByUser(userId)); // finds all users a user like.
    }

    
    public List<DashboardPostDTO> GetAllPostByLikedUsers(int userId, int skip, int take)
    {
        var list = _mapper.Map<List<User>>(_repo.GetAllLikedUsersByUser(userId)); //finds all the users that a user likes
        
        return _repo.GetAllPostByLikedUsers(list, skip, take); // gets all the DashboardPostDTO
    }
}