namespace Application.DTOs;

public class UserRegisterDTO
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateOnly Birthday { get; set; }
    public string? ProfilePicture { get; set; }
    public string? Role { get; set; }
    
}