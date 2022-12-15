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
    
    //get patterns by id
    public Pattern GetPatternById(int id);

    // get all the patterns owned by a user
    public List<Pattern> GetAllPatternsByUser(int userId);
}