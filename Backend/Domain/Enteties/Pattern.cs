namespace Domain;

public class Pattern
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public String PdfString { get; set; }
    public String Description { get; set; }
    public String Image { get; set; }
}