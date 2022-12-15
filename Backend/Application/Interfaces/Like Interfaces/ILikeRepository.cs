
using Application.DTOs.Like;

using Application.DTOs;

using Domain;

namespace Application.Interfaces.Like_Interfaces;

public interface ILikeRepository
{
    
    public Like LikeUser(Like like);
    /// <summary>
    /// removes a relation where a user likes another user.
    /// </summary>
    /// <param name="like">object holding both users for the relation, user and likeduser</param>
    /// <returns></returns>
    public Like RemoveLike(Like like);

    /// <summary>
    /// checks if a user already likes another user
    /// </summary>
    /// <param name="like">like object holding user and likeduser</param>
    /// <returns>returns the like if exist otherwise null</returns>
    public Like AlreadyLikes(Like like);
    
    /// <summary>
    /// finds all users that a user likes
    /// </summary>
    /// <param name="userId">user id of person who likes other users</param>
    /// <returns>list of like objects that holds the users whom a user likes</returns>
    public List<Like> GetAllLikedUsersByUser(int userId);
    
    /// <summary>
    ///  finds a list of DashboardPostDTO from all users that a user likes.
    /// </summary>
    /// <param name="users">list of users a user like</param>
    /// <param name="skip"> amount of indexs skipped in list before starting list</param>
    /// <param name="take">amount of objects in list</param>
    /// <returns></returns>
    public List<DashboardPostDTO> GetAllPostByLikedUsers(List<User> users, int skip, int take);

    /// <summary>
    /// checks if both users exist. user wants to like another user.
    /// </summary>
    /// <param name="dto">holds id of user whom wants to like someone, and id person thats liked</param>
    /// <returns></returns>
    bool DoesUserExist(SimpleLikeDto dto);
}