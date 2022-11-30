namespace Application.DTOs;

public class PatternGetAllDTO
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    public UserDTO User { get; set; }
    public string Image { get; set; }
    
}