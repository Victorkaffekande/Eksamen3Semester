namespace Application.DTOs;

public class PostCreateDTO
{
    public int ProjectId { get; set; }
    
    public String Description { get; set; }
    public DateTime PostDate { get; set; }
    public String Image { get; set; }
}