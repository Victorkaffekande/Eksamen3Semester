namespace Application.DTOs;

public class ProjectGetAllDTO
{
    public int Id { get; set; }
    
    public UserDTO User { get; set; }
    public  String? Image { get; set; }
    public  String Title { get; set; }
    
}