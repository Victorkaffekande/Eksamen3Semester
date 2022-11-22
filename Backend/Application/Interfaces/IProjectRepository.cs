﻿using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IProjectRepository
{
    List<Project> GetAllProjects();
    Project AddProject(Project project);
    Project UpdateProject(Project project);
    Project GetProjectById(int id);
}