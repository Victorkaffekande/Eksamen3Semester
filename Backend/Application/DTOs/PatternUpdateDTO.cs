using Domain;

namespace Application.DTOs;

public class PatternUpdateDTO
{
    public string Title { get; set; }
    public int Id { get; set; }
    public int UserId { get; set; }
    
    public String PdfString { get; set; }
    public String Description { get; set; }
    public String Image { get; set; }
}