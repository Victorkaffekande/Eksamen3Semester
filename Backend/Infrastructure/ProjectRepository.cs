using Application.DTOs;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ProjectRepository : IProjectRepository
{
    private DatabaseContext _context;

    public ProjectRepository(DatabaseContext context)
    {
        _context = context;
    }
    

    public Project AddProject(Project project)
    {
        _context.ProjectTable.Add(project);
        _context.SaveChanges();
        return project;
    }

    public Project UpdateProject(Project project)
    {
        _context.Update(project);
        return project;
    }

    public Project GetProjectById(int id)
    {
        return _context.ProjectTable.Find(id) ?? throw new ArgumentException("Project does not exist");
    }

    public Project DeleteProject(Project project)
    {
        _context.ProjectTable.Remove(project);
        _context.SaveChanges();
        return project;
    }
}