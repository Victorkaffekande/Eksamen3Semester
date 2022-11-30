namespace Application.DTOs;

public class PostGetAllDTO
{
    public int Id { get; set; }

    public ProjectGetAllDTO Project { get; set; }

    public String Description { get; set; }
    public DateTime PostDate { get; set; }
    public String Image { get; set; }
}