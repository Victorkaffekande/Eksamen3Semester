using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IProjectService
{
    //crud functions
    /// <summary>
    /// creates a new project
    /// </summary>
    /// <param name="projectCreateDto">information needed to create project</param>
    /// <returns>created project</returns>
    Project CreateProject(ProjectCreateDTO projectCreateDto);
    
    /// <summary>
    /// updates a project
    /// </summary>
    /// <param name="project">the project for update</param>
    /// <returns> the created project</returns>
    Project UpdateProject(Project project);
    /// <summary>
    /// get all projects from database
    /// </summary>
    /// <returns>list of projects</returns>
    List<Project> GetAllProjects();
    /// <summary>
    /// deletes a project
    /// </summary>
    /// <param name="id"> project id</param>
    /// <returns>project</returns>
    Project DeleteProject(int id);
    
    /// <summary>
    /// get project by id
    /// </summary>
    /// <param name="id">the project id</param>
    /// <returns></returns>
    Project GetProjectById(int id);
    
    /// <summary>
    /// gets all projects that belongs to a user
    /// </summary>
    /// <param name="id">id of the user</param>
    /// <returns></returns>
    List<Project> GetAllProjectsFromUser(int id);

}