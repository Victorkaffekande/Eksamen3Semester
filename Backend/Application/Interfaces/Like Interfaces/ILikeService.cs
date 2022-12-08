using Application.DTOs;
using Application.DTOs.Like;
using Domain;

namespace Application.Interfaces.Like_Interfaces;

public interface ILikeService
{
    public SimpleLikeDto LikeUser(SimpleLikeDto dto);
    public SimpleLikeDto RemoveLike(SimpleLikeDto dto);
    
    public bool AlreadyLike(SimpleLikeDto dto);
    public List<UserDTO> GetAllLikedUsersByUser(int userId);

    public List<DashboardPostDTO> GetAllPostByLikedUsers(int userId, int skip, int take);
}