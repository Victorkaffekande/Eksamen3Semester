using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace Application;

public class ProjectService : IProjectService
{
    private IProjectRepository _repo;
    private IMapper _mapper;
    private ProjectDTOValidator _dtoValidator;
    private ProjectValidator _projectValidator;

    public ProjectService(IProjectRepository repo,
        IMapper mapper,
        ProjectDTOValidator dtoValidator,
        ProjectValidator projectValidator)
    {
        _repo = repo ?? throw new ArgumentException("Missing ProjectRepository");
        _mapper = mapper ?? throw new ArgumentException("Missing Mapper");
        _dtoValidator = dtoValidator ?? throw new ArgumentException("Missing DTO Validator");
        _projectValidator = projectValidator ?? throw new ArgumentException("Missing project Validator");
    }

    public Project CreateProject(ProjectCreateDTO projectCreateDto)
    {
        if (projectCreateDto == null) throw new ArgumentException("ProjectDTO is null");

        var val = _dtoValidator.Validate(projectCreateDto);
        if (!val.IsValid)
        {
            throw new ArgumentException(val.ToString());
        }

        return _repo.AddProject(_mapper.Map<Project>(projectCreateDto));
    }

    public Project UpdateProject(Project project)
    {
        if (project == null) throw new ArgumentException("Project is null");

        var val = _projectValidator.Validate(project);
        if (!val.IsValid) throw new ArgumentException(val.ToString());

        if (_repo.GetProjectById(project.Id) == null) throw new AggregateException("Project does not exist");

        return _repo.UpdateProject(project);
    }

    public Project GetProjectById(int id)
    {
        if (id < 1) throw new ArgumentException("Project Id must be 1 or above");
        return _repo.GetProjectById(id);
    }

    public List<Project> GetAllProjectsFromUser(int id)
    {
        if (id < 1) throw new ArgumentException("Id cannot be lower than 1");

            return _repo.GetAllProjectsFromUser(id);
    }

    public List<Project> GetAllProjects()
    {
        return _repo.GetAllProjects();
    }

    public Project DeleteProject(int id)
    {
        var p = GetProjectById(id);
        
        if (p == null) throw new ArgumentException("Project id does not exist");
        
        return _repo.DeleteProject(p);
    }
}