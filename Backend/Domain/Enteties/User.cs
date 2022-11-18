namespace Domain;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public string? ProfilePicture { get; set; }
    public DateOnly BirthDay { get; set; }
    
    public List<Pattern>? Patterns { get; set; }
    public List<Project>? Projects { get; set; }
}