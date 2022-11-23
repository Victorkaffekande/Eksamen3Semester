using Application.DTOs;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ProjectRepository : IProjectRepository
{
    private DbContext _context;

    public ProjectRepository(DbContext context)
    {
        _context = context;
    }

    public List<Project> GetAllProjects()
    {
        throw new NotImplementedException();
    }

    public Project AddProject(Project project)
    {
        throw new NotImplementedException();
    }

    public Project UpdateProject(Project project)
    {
        throw new NotImplementedException();
    }

    public Project GetProjectById(int id)
    {
        throw new NotImplementedException();
    }

    public Project DeleteProject(Project project)
    {
        throw new NotImplementedException();
    }
}