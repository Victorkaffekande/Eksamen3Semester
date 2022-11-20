using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IProjectService
{
    Project CreateProject(ProjectDTO projectDto);

}