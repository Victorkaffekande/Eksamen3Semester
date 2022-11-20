using Domain;

namespace Application.DTOs;

public class ProjectDTO
{
    public User User { get; set; }
    public int UserId { get; set; }

    public Pattern? Pattern { get; set; }
    public int? PatternId { get; set; }

    public String? Image { get; set; }
    public String Title { get; set; }
    public DateTime StartTime { get; set; }
    public Boolean IsActive { get; set; }
    
    public List<Post>? Posts { get; set; }

    public ProjectDTO(User user, int userId, Pattern? pattern, int? patternId, string image, string title, DateTime startTime, bool isActive)
    {
        User = user;
        UserId = userId;
        Pattern = pattern;
        PatternId = patternId;
        Image = image;
        Title = title;
        StartTime = startTime;
        IsActive = isActive;
    }
}