using Domain;

namespace Application.Interfaces;

public interface IPatternRepository
{
    //crud functions
    public List<Pattern> GetAllPattern();
    public Pattern CreatePattern(Pattern pattern);
    public Pattern UpdatePattern(Pattern pattern);
    public Pattern DeletePattern(Pattern pattern);
    
}