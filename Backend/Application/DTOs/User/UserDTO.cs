namespace Application.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string? Email { get; set; }
    public string? ProfilePicture { get; set; }
    public DateOnly BirthDay { get; set; }
}