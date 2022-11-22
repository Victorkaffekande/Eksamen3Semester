using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;
using FluentValidation;

namespace Application;

public class PatternService : IPatternService
{
    
    private IPatternRepository _repo;
    private IMapper _mapper;
    private PatternDTOValidator _dtoValidator;
    private PatternValidator _patternValidator;
    public PatternService(IPatternRepository repo, IMapper mapper, PatternDTOValidator dtoValidator, PatternValidator patternValidator)
    {
        _repo = repo ?? throw new ArgumentException("Missing Repository");
        _mapper = mapper ?? throw new ArgumentException("Missing Mapper");
        _dtoValidator = dtoValidator ?? throw new ArgumentException("Missing Validator");
        _patternValidator = patternValidator ?? throw new ArgumentException("Missing Validator");
    }

    public List<Pattern> GetAllPattern()
    {
        return _repo.GetAllPattern();
    }

    public Pattern CreatePattern(PatternDTO dto)
    {
        if (dto is null) throw new ArgumentException("PatternDTO is null");
        

            var val = _dtoValidator.Validate(dto);
        if (!val.IsValid) throw new ArgumentException(val.ToString());


        return _repo.CreatePattern(_mapper.Map<Pattern>(dto));
    }

    public Pattern UpdatePattern(Pattern pattern)
    {
        if (pattern is null) throw new ArgumentException("Pattern is null");
        

        var val = _patternValidator.Validate(pattern);
        if (!val.IsValid) throw new ArgumentException(val.ToString());

        if (_repo.GetPatternById(pattern.Id) == null) throw new ArgumentException("pattern id does not exist");

            return _repo.UpdatePattern(pattern);
    }

    public Pattern DeletePattern(int id)
    {
        throw new NotImplementedException();
    }

    public Pattern GetPatternById(int id)
    {
        if (id<1) throw new ArgumentException("Id cannot be lower than 1");
        
            return _repo.GetPatternById(id);
    }
}