using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure;

public class PatternRepository : IPatternRepository
{
    private DatabaseContext _context;

    public PatternRepository(DatabaseContext context)
    {
        _context = context;
    }

    public List<Pattern> GetAllPattern()
    {
       return _context.PatternTable.Include(p => p.User).ToList();

        //return _context.PatternTable.ToList();
    }

    public Pattern CreatePattern(Pattern pattern)
    {
        _context.PatternTable.Add(pattern);
        _context.SaveChanges();
        return pattern;
    }

    public Pattern UpdatePattern(Pattern pattern)
    {
        throw new NotImplementedException();
    }

    public Pattern DeletePattern(Pattern pattern)
    {
        throw new NotImplementedException();
    }
}