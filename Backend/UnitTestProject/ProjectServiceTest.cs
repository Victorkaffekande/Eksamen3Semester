using System.Collections.Immutable;
using System.Security.Cryptography.X509Certificates;
using Application;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace UnitTestProject;

public class ProjectServiceTest
{
    private List<Project> fakeRepo = new List<Project>();
    private Mapper _mapper;
    private Mock<IProjectRepository> _mockRepo;
    private ProjectDTOValidator _dtoValidator;
    private ProjectValidator _projectValidator;

    //Constructor is run for every single test
    public ProjectServiceTest()
    {
        //mapper setup
        var config = new MapperConfiguration(cfg => { cfg.CreateMap<ProjectDTO, Project>(); });
        _mapper = new Mapper(config);

        //Mockrepo setup
        _mockRepo = new Mock<IProjectRepository>();
        _mockRepo.Setup(r => r.GetAllProjects()).Returns(fakeRepo);
        _mockRepo.Setup(r => r.AddProject(It.IsAny<Project>())).Callback<Project>(p => fakeRepo.Add(p));
        _mockRepo.Setup(r => r.GetProjectById(It.IsAny<int>())).Returns<int>(
            (id) => fakeRepo.FirstOrDefault(x => x.Id == id));
        _mockRepo.Setup(r => r.UpdateProject(It.IsAny<Project>())).Callback<Project>(p =>
        {
            var index = fakeRepo.FindIndex(other => other.Id == p.Id);
            if (index != -1)
                fakeRepo[index] = p;
        });

        //Validator setup
        _dtoValidator = new ProjectDTOValidator();
        _projectValidator = new ProjectValidator();
    }

    #region Constructor tests

// Valid create test
    [Fact]
    public void CreateProjectService_Valid()
    {
        //arrange
        IProjectRepository repo = _mockRepo.Object;

        //act
        IProjectService service = new ProjectService(repo, _mapper, _dtoValidator, _projectValidator);

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
        var ex = Assert.Throws<ArgumentException>(() =>
            service = new ProjectService(null, _mapper, _dtoValidator, _projectValidator));
        Assert.Equal("Missing ProjectRepository", ex.Message);
    }

    [Fact]
    public void CreateProjectService_NullMapper()
    {
        //Arrange
        IProjectRepository repo = _mockRepo.Object;
        ProjectService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() =>
            service = new ProjectService(repo, null, _dtoValidator, _projectValidator));
        Assert.Equal("Missing Mapper", ex.Message);
    }

    [Fact]
    public void CreateProjectService_NullDtoValidator()
    {
        //Arrange
        IProjectRepository repo = _mockRepo.Object;
        ProjectService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() =>
            service = new ProjectService(repo, _mapper, null, _projectValidator));
        Assert.Equal("Missing DTO Validator", ex.Message);
    }

    [Fact]
    public void CreateProjectService_NullProjectValidator()
    {
        //Arrange
        IProjectRepository repo = _mockRepo.Object;
        ProjectService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(
            () => service = new ProjectService(repo, _mapper, _dtoValidator, null));
        Assert.Equal("Missing project Validator", ex.Message);
    }

    #endregion


    #region Create project tests

    [Theory]
    [InlineData(1, "data:image/png;base64,fillerData")] //png file
    [InlineData(1, "data:image/jpeg;base64,fillerData")] //jpeg file
    [InlineData(0, "data:image/jpeg;base64,fillerData")] //No patternId
    [InlineData(1, null)] //Null image
    public void CreateProject_Valid(int patternId, string image)
    {
        //arrange
        var dto = new ProjectDTO()
        {
            UserId = 1, PatternId = 1, Image = null, Title = "Filler title",
            IsActive = true, StartTime = new DateTime()
        };
        var repo = _mockRepo.Object;
        var service = new ProjectService(repo, _mapper, _dtoValidator, _projectValidator);

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
        var service = new ProjectService(repo, _mapper, _dtoValidator, _projectValidator);

        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.CreateProject(null));
        Assert.Equal("ProjectDTO is null", ex.Message);
        _mockRepo.Verify(r => r.AddProject(null), Times.Never);
    }

    [Theory]
    [InlineData(0, "title", "2022-05-02", "data:image/png;base64,fillerData",
        "User id must be 1 or higher")] //user id cannot be 0 or lower
    [InlineData(1, "title", "2022-05-02", "data:application/pdf;base64,fillerData",
        "Only PNG and JPG files are allowed")] // PDF in the img input
    [InlineData(1, null, "2022-05-02", "data:image/png;base64,fillerData",
        "Title can not be empty or null")] //Null title
    [InlineData(1, " ", "2022-05-02", "data:image/png;base64,fillerData",
        "Title can not be empty or null")] //blank title
    public void CreateProject_invalid_ProjectDTO(int userId, string title, string startDate, string img,
        string errorMsg)
    {
        //arrange
        var dto = new ProjectDTO()
            { UserId = userId, Title = title, StartTime = DateTime.Parse(startDate), Image = img };


        var service = new ProjectService(_mockRepo.Object, _mapper, _dtoValidator, _projectValidator);

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.CreateProject(dto));
        Assert.Equal(errorMsg, ex.Message);
        _mockRepo.Verify(r => r.AddProject(It.IsAny<Project>()), Times.Never);
    }

    #endregion

    #region Update project tests

    [Theory]
    [InlineData(1, "data:image/png;base64,fillerData")] //png file
    [InlineData(1, "data:image/jpeg;base64,fillerData")] //jpeg file
    [InlineData(0, "data:image/jpeg;base64,fillerData")] //No patternId
    [InlineData(1, null)] //Null image
    //validate if pat id null => null pat
    public void UpdateProject_ValidProject(int patternId, string imgString)
    {
        //arrange
        var service = new ProjectService(_mockRepo.Object, _mapper, _dtoValidator, _projectValidator);
        var oldProject = new Project()
        {
            Id = 1,
            UserId = 1,
            Title = "old project",
            Image = "imgString",
            IsActive = true,
            PatternId = 5,
            StartTime = DateTime.Now
        };
        _mockRepo.Object.AddProject(oldProject);
        var newProject = new Project()
        {
            Id = 1,
            UserId = 1,
            Title = "new project",
            Image = imgString,
            IsActive = true,
            PatternId = patternId,
            StartTime = DateTime.Now
        };

        //act
        service.UpdateProject(newProject);

        //assert
        Assert.True(fakeRepo.Count == 1);
        Assert.Equal(newProject, fakeRepo[0]);
        _mockRepo.Verify(r => r.UpdateProject(newProject), Times.Once);
    }

    [Theory]
    [InlineData(null, "data:image/png;base64,fillerData", 1, "Title can not be empty or null")] // null title
    [InlineData(" ", "data:image/png;base64,fillerData", 1, "Title can not be empty or null")] // empty title
    [InlineData("title", "data:application/pdf;base64,fillerData", 1,
        "Only PNG and JPG files are allowed")] //wrong file type
    [InlineData("title", "data:image/png;base64,fillerData", null,
        "Project must have a pattern Id, if a pattern is connected")] // no pattern id
    public void UpdateProject_InvalidProject(String title, string image, int patternId, string errorMsg)
    {
        //arrange
        var service = new ProjectService(_mockRepo.Object, _mapper, _dtoValidator, _projectValidator);
        Pattern? pat = null;
        if (patternId != null) pat = new Pattern();
        var newProject = new Project()
        {
            Id = 1,
            UserId = 1,
            Title = title,
            Image = image,
            IsActive = true,
            PatternId = patternId,
            Pattern = pat,
            StartTime = DateTime.Now
        };

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdateProject(newProject));
        Assert.Equal(errorMsg, ex.Message);
        _mockRepo.Verify(r => r.UpdateProject(It.IsAny<Project>()), Times.Never);
    }

    [Fact] //null input
    public void UpdateProject_Invalid_NullProject()
    {
        //arrange
        var service = new ProjectService(_mockRepo.Object, _mapper, _dtoValidator, _projectValidator);

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdateProject(null));
        Assert.Equal("Project is null", ex.Message);
        _mockRepo.Verify(r => r.UpdateProject(It.IsAny<Project>()), Times.Never);
    }

    //TODO LAV GET BY ID OG KOM TILBAGE TIL DEN HER
    [Fact] //findes ikke i repo
    public void UpdateProject_Invalid_ProjectDoesNotExistInRepo()
    {
        //arrange
        var service = new ProjectService(_mockRepo.Object, _mapper, _dtoValidator, _projectValidator);
        var oldProject = new Project()
        {
            Id = 1,
            UserId = 1,
            Title = "oldTitle",
            Image = "data:image/png;base64,fillerData",
            IsActive = true,
            StartTime = DateTime.Now
        };
        _mockRepo.Object.AddProject(oldProject);
        var newProject = new Project()
        {
            Id = 2,
            UserId = 1,
            Title = "oldTitle",
            Image = "data:image/png;base64,fillerData",
            IsActive = true,
            StartTime = DateTime.Now
        };

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdateProject(newProject));
        Assert.Equal("Project does not exist", ex.Message);
        _mockRepo.Verify(r => r.UpdateProject(It.IsAny<Project>()), Times.Never);
    }

    #endregion

    #region get By Id Tests
//getproject by id - existing project
//getproject by id - no project in db
    #endregion
}