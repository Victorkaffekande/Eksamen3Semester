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
    private PatternValidator _validator;
    public PatternService(IPatternRepository repo, IMapper mapper, PatternValidator validator)
    {
        _repo = repo ?? throw new ArgumentException("Missing Repository");
        _mapper = mapper ?? throw new ArgumentException("Missing Mapper");
        _validator = validator ?? throw new ArgumentException("Missing Validator");
    }

    public List<Pattern> GetAllPattern()
    {
        throw new NotImplementedException();
    }

    public Pattern CreatePattern(PatternDTO dto)
    {
        if (dto is null) throw new ArgumentException("PatternDTO is null");
        

            var val = _validator.Validate(dto);
        if (!val.IsValid) throw new ValidationException(val.ToString());


        return _repo.CreatePattern(_mapper.Map<Pattern>(dto));
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