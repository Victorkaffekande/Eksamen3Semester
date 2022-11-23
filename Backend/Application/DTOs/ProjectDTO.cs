using Domain;

namespace Application.DTOs;

public class ProjectDTO
{
    public int UserId { get; set; }
    public int? PatternId { get; set; }
    public String? Image { get; set; }
    public String Title { get; set; }
    public DateTime StartTime { get; set; }
    public Boolean IsActive { get; set; }
}