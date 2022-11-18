namespace Domain;

public class Post
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public String Description { get; set; }
    public DateTime PostDate { get; set; }
    public String Image { get; set; }
}