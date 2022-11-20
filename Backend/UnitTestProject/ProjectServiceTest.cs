﻿using System.Collections.Immutable;
using System.Security.Cryptography.X509Certificates;
using Application;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace UnitTestProject;

public class ProjectServiceTest
{
    private List<Project> fakeRepo = new List<Project>();
    private Mapper _mapper;
    private Mock<IProjectRepository> _mockRepo;
    private ProjectValidator _validator;

    //Constructor is run for every single test
    public ProjectServiceTest()
    {
        //mapper setup
        var config = new MapperConfiguration(cfg => { cfg.CreateMap<ProjectDTO, Project>(); });
        _mapper = new Mapper(config);

        //Mockrepo setup
        _mockRepo = new Mock<IProjectRepository>();
        _mockRepo.Setup(r => r.GetAllProjects()).Returns(fakeRepo);
        _mockRepo.Setup(r => r.AddProject(It.IsAny<Project>())).Callback<Project>(p => fakeRepo.Add(p)); //måske fucked

        //Validator setup
        _validator = new ProjectValidator();
    }

    #region Constructor tests

// Valid create test
    [Fact]
    public void CreateProjectService_Valid()
    {
        //arrange
        IProjectRepository repo = _mockRepo.Object;

        //act
        IProjectService service = new ProjectService(repo, _mapper, _validator);

        //assert
        Assert.NotNull(service);
        Assert.True(service is ProjectService);
    }


    //Invalid create tests
    [Fact]
    public void CreateProjectService_NullRepo()
    {
        //Arrange
        ProjectService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service = new ProjectService(null, _mapper, _validator));
        Assert.Equal("Missing ProjectRepository", ex.Message);
    }

    [Fact]
    public void CreateProjectService_NullMapper()
    {
        //Arrange
        IProjectRepository repo = _mockRepo.Object;
        ProjectService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service = new ProjectService(repo, null, _validator));
        Assert.Equal("Missing Mapper", ex.Message);
    }

    [Fact]
    public void CreateProjectService_NullValidator()
    {
        //Arrange
        IProjectRepository repo = _mockRepo.Object;
        ProjectService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service = new ProjectService(repo, _mapper, null));
        Assert.Equal("Missing Validator", ex.Message);
    }

    #endregion


    /*
     * TODO LAV MEMBERDATA FOR:
            //alt udfyldt
            //no pattern
            //no image
     */
    [Fact]
    public void CreateProject_Valid()
    {
        //arrange
        var user = new User(1, "TimmyCool", null, null, DateOnly.FromDateTime(DateTime.Today));
        var p = new Pattern();
        var dto = new ProjectDTO(user, user.Id, p, p.Id, "Filler image string", "Project Title", DateTime.Now,
            true);
        var repo = _mockRepo.Object;
        var service = new ProjectService(repo, _mapper, _validator);

        //act
        service.CreateProject(dto);

        //assert
        Assert.NotNull(fakeRepo[0]);
        _mockRepo.Verify(r => r.AddProject(It.IsAny<Project>()), Times.Once);
    }

    [Fact]
    public void createProject_Invalid_NullDto()
    {
        //arrange
        var repo = _mockRepo.Object;
        var service = new ProjectService(repo, _mapper, _validator);

        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.CreateProject(null));
        Assert.Equal("ProjectDTO is null", ex.Message);
        _mockRepo.Verify(r => r.AddProject(null), Times.Never);
    }
    
    /**
     * TODO LAV MEMBERDATA
     * Null user
     * Null title
     * null startdate
     */
    [Fact]
    public void CreateProject_invalid_ProjectDTO()
    {
        
    }
}