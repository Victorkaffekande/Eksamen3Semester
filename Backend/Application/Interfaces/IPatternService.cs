using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IPatternService
{
    //crud functions
    public List<PatternGetAllDTO> GetAllPattern();
    public Pattern CreatePattern(PatternDTO dto);
    public Pattern UpdatePattern(PatternUpdateDTO dto);
    public Pattern DeletePattern(int id);
    
    public Pattern GetPatternById(int id);

    public List<Pattern> GetAllPatternsByUser(int userId);
}