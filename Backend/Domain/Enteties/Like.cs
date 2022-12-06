namespace Domain;

public class Like
{
    public User? User { get; set; }
    public int UserId { get; set; }
    
    public User? LikedUser { get; set; }
    public int LikedUserId { get; set; }
}