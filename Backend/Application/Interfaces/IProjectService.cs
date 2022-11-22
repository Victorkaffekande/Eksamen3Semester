using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IProjectService
{
    Project CreateProject(ProjectDTO projectDto);
    Project UpdateProject(Project project);
    Project GetProjectById(int id);
}