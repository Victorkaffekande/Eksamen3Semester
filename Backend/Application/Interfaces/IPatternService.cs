using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IPatternService
{
    //crud functions
    public List<Pattern> GetAllPattern();
    public Pattern CreatePattern(PatternDTO dto);
    public Pattern UpdatePattern(Pattern pattern);
    public Pattern DeletePattern(Pattern pattern);
    
}