using Application;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;
using Moq;
using Newtonsoft.Json;

namespace UnitTestProject;

public class UserServiceTest
{
    
    private List<User> fakeRepo = new List<User>();
    private Mock<IUserRepository> _mockRepo;
    
    //Constructor is run for every single test

    public UserServiceTest()
    {
        
        //Mockrepo setup
        _mockRepo = new Mock<IUserRepository>();
     
    }
    
    #region GetPatternByIdTests
    
    [Fact]
    public void GetUserById_ExistingUser_Test()
    {
        // Arrange
        var id = 1;
        var existingUser = new UserDTO()
        {
            Id = id,
            Email = "",
            BirthDay = DateOnly.MinValue,
            ProfilePicture = null,
            Username = ""
        };

        var dbUser = new User()
        {
            Id = id,
            BirthDay = DateOnly.MinValue,
            Email = "",
            Password = "",
            Patterns = null,
            ProfilePicture = null,
            Projects = null,
            Role = "",
            Salt = "",
            Username = "",

        };
        var service = new UserService(_mockRepo.Object);
        _mockRepo.Setup(r => r.GetUserById(id)).Returns(dbUser);

        // Act
        var result = service.GetUserById(id);

        var expected = JsonConvert.SerializeObject(existingUser);
        var actual = JsonConvert.SerializeObject(result);

        
        // Assert
        Assert.Equal(expected, actual);
        _mockRepo.Verify(r => r.GetUserById(id), Times.Once);
    }
    
    [Fact]
    public void GetPatternById_InvalidId_Test()
    {
        // Arrange
        var id = 0;
        string error = "Id cannot be lower than 1";

        var service = new UserService(_mockRepo.Object);

        // Assert & act
        var ex = Assert.Throws<ArgumentException>(() => service.GetUserById(id));
        Assert.Equal(error, ex.Message);
        //_mockRepo.Verify(r => r.GetUserById(id), Times.Never);
    }
    
    [Fact]
    public void GetPatternById_NonExistingPattern_Test()
    {
        // Arrange
        var id = 1;
        
        var service = new UserService(_mockRepo.Object);
        _mockRepo.Setup(r => r.GetUserById(id)).Returns(() => null);

        // Act
        var result = service.GetUserById(id);

        // Assert
        Assert.Null(result);
        _mockRepo.Verify(r => r.GetUserById(id), Times.Once);
    }

    #endregion //GetPatternById_Tests

    

}