
using Application.DTOs.Like;

using Application.DTOs;

using Domain;

namespace Application.Interfaces.Like_Interfaces;

public interface ILikeRepository
{
    
    public Like LikeUser(Like like);
    public Like RemoveLike(Like like);

    public Like AlreadyLikes(Like like);
    public List<Like> GetAllLikedUsersByUser(int userId);
    public List<DashboardPostDTO> GetAllPostByLikedUsers(List<User> users, int skip, int take);

    bool DoesUserExist(SimpleLikeDto dto);
}