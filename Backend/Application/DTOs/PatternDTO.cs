using Domain;

namespace Application.DTOs;

public class PatternDTO
{
    public int UserId { get; set; }
    public User User { get; set; }
    
    public String PdfString { get; set; }
    public String Description { get; set; }
    public String Image { get; set; }
}