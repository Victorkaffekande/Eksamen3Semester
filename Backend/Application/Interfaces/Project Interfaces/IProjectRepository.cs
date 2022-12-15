using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IProjectRepository
{
    //crud functions
    Project AddProject(Project project);
    Project UpdateProject(Project project);
    List<Project> GetAllProjects();
    Project DeleteProject(Project project);
    
    /// <summary>
    /// get a project by id
    /// </summary>
    /// <param name="id"> id of project</param>
    /// <returns>the project</returns>
    Project GetProjectById(int id);
    
    /// <summary>
    /// gets all projects from a user
    /// </summary>
    /// <param name="id">id of the user</param>
    /// <returns>all projects</returns>
    List<Project> GetAllProjectsFromUser(int id);
    
    
}