namespace Application.DTOs;

public class DashboardPostDTO
{
    //post
    public int Id { get; set; }
    public String Description { get; set; } 
    public DateTime PostDate { get; set; }
    public String Image { get; set; }
    
    //project

    public string Title{ get; set; }
    public int ProjectId{ get; set; }
    
    //user

    public string Username { get; set; }
    public string ProfilePicture { get; set; }
    public int UserId { set; get; }
}