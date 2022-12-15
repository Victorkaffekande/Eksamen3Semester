using Application.DTOs;
using Application.DTOs.Like;
using Domain;

namespace Application.Interfaces.Like_Interfaces;

public interface ILikeService
{
    /// <summary>
    /// creates a relation between 2 users. so one user likes another user.
    /// </summary>
    /// <param name="dto">holds the id of the user whom wants to like another user, and the id of the user who is liked</param>
    /// <returns></returns>
    public SimpleLikeDto LikeUser(SimpleLikeDto dto);
    /// <summary>
    /// removes a like relation between 2 users
    /// </summary>
    /// <param name="dto">holds an id of person who like another person, and the id of the liked person</param>
    /// <returns>the object back</returns>
    public SimpleLikeDto RemoveLike(SimpleLikeDto dto);
    
    public bool AlreadyLike(SimpleLikeDto dto);
    /// <summary>
    /// gets all users that a user likes
    /// </summary>
    /// <param name="userId"> the user id from whom to find all users that user like</param>
    /// <returns>a list of userdto so not to return password</returns>
    public List<UserDTO> GetAllLikedUsersByUser(int userId);

    /// <summary>
    ///  finds a list of DashboardPostDTO from all users that a user likes.
    /// </summary>
    /// <param name="userId">the user id of the person from where to find all the users that user likes</param>
    /// <param name="skip">the amount of index that should be skipped before starting the list</param>
    /// <param name="take">the amount of DashboardPostDto the list to have</param>
    /// <returns>
    /// returns a list of DashboardPostDTO that combines a post with information about project and user.
    /// </returns>
    public List<DashboardPostDTO> GetAllPostByLikedUsers(int userId, int skip, int take);
}