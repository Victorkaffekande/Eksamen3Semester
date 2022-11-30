﻿using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IProjectRepository
{
    Project AddProject(Project project);
    Project UpdateProject(Project project);
    Project GetProjectById(int id);
    List<Project> GetAllProjectsFromUser(int id);
    Project DeleteProject(Project project);
}