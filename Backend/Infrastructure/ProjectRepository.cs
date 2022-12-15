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
        var foundP = GetProjectById(project.Id);

        if (foundP.Id == project.Id)
        {
            foundP.Image = project.Image;
            foundP.Title = project.Title;
            foundP.IsActive = project.IsActive;
            foundP.PatternId = project.PatternId;
        }

        _context.ProjectTable.Update(foundP);
        _context.SaveChanges();
        return project;
    }

    public List<Project> GetAllProjectsFromUser(int id)
    {
        return _context.ProjectTable.Where(p => p.UserId == id).ToList();
    }

    public List<Project> GetAllProjects()
    {
        return _context.ProjectTable.ToList();
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