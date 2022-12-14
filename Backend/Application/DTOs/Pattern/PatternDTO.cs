using Domain;

namespace Application.DTOs;

public class PatternDTO
{
    
    public string Title { get; set; }
    public int UserId { get; set; }
    public String PdfString { get; set; }
    public String Description { get; set; }
    public String Image { get; set; }
    
    public string? difficulty { get; set; }
    public string? yarn { get; set; }
    public string? Language { get; set; }
    public string? Needlesize { get; set; }
    public string? gauge { get; set; }
}