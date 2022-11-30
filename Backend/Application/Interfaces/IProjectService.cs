﻿using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IProjectService
{
    Project CreateProject(ProjectCreateDTO projectCreateDto);
    Project UpdateProject(Project project);
    Project GetProjectById(int id);
    List<Project> GetAllProjectsFromUser(int id);
    Project DeleteProject(int id);
}