namespace Domain;

public class Pattern
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public string PdfString { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    
    
    public string? Difficulty { get; set; }
    
    public string? Yarn { get; set; }
    
    public string? Language { get; set; }
    
    public string? NeedleSize { get; set; }
    
    public string? Gauge { get; set; }

    public List<Project>? Projects { get; set; }

}