using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;

namespace Application;

public class ProjectService : IProjectService
{
    private IProjectRepository _repo;
    private IMapper _mapper;
    private ProjectValidator _validator;

    public ProjectService(IProjectRepository repo, IMapper mapper, ProjectValidator validator)
    {
        _repo = repo ?? throw new ArgumentException("Missing ProjectRepository");
        _mapper = mapper ?? throw new ArgumentException("Missing Mapper");
        _validator = validator ?? throw new ArgumentException("Missing Validator");
    }

    public Project CreateProject(ProjectDTO projectDto)
    {
        if (projectDto == null) throw new ArgumentException("ProjectDTO is null");
        
        return _repo.AddProject(_mapper.Map<Project>(projectDto));
    }
}