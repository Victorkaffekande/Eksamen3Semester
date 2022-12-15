using Domain;

namespace Application.Interfaces;

public interface IPatternRepository
{
    //crud functions
    public List<Pattern> GetAllPattern();
    public Pattern CreatePattern(Pattern pattern);
    public Pattern UpdatePattern(Pattern pattern);
    public Pattern DeletePattern(int id);

    //finds pattern by id
    public Pattern GetPatternById(int id);

    //gets all patterns that a user owns
    public List<Pattern> GetAllPatternsByUser(int userId);

}