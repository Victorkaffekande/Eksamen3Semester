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

    }

    public Pattern CreatePattern(Pattern pattern)
    {
        _context.PatternTable.Add(pattern);
        _context.SaveChanges();
        return pattern;
    }

    public Pattern UpdatePattern(Pattern pattern)
    {
        var oldPattern = GetPatternById(pattern.Id);
        if (oldPattern.Id.Equals(pattern.Id))
        {
            oldPattern.Title = pattern.Title;
            oldPattern.Description = pattern.Description;
            oldPattern.Image = pattern.Image;
            oldPattern.PdfString = pattern.PdfString;
            oldPattern.Difficulty = pattern.Difficulty;
            oldPattern.Yarn = pattern.Yarn;
            oldPattern.Language = pattern.Language;
            oldPattern.NeedleSize = pattern.NeedleSize;
            oldPattern.Gauge = pattern.Gauge;
        }

        _context.PatternTable.Update(oldPattern ?? throw new InvalidOperationException());
        _context.SaveChanges();
        return oldPattern;
    }

    public Pattern DeletePattern(int id)
    {
        var pattern = _context.PatternTable.Find(id);
        _context.PatternTable.Remove(pattern ?? throw new InvalidOperationException());
        _context.SaveChanges();
        return pattern;
    }

    public Pattern GetPatternById(int id)
    {
        return _context.PatternTable.Find(id);
    }

    public List<Pattern> GetAllPatternsByUser(int userId)
    {
        return _context.PatternTable.Where(p => p.UserId == userId).ToList();
    }
}