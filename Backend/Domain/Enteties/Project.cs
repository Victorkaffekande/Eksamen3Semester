namespace Domain;

public class Project
{
    public int Id { get; set; }
    
    public User User { get; set; }
    public  int UserId { get; set; }
    
    public Pattern? Pattern { get; set; }
    public  int? PatternId { get; set; }
    
    public List<Post>? Posts { get; set; }
    
    public  String? Image { get; set; }
    public  String Title { get; set; }
    public  DateTime StartTime { get; set; }
    public Boolean IsActive { get; set; }
}