using Application;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;
using Infrastructure;
using Moq;
using Newtonsoft.Json;

namespace UnitTestProject;

public class UserServiceTest
{
    
    private List<User> _fakeRepo = new List<User>();
    private Mapper _mapper;
    private UserDTOValidator _userDtoValidator;
    private Mock<IUserRepository> _mockRepo;
    
    //Constructor is run for every single test

    public UserServiceTest()
    {
     
        //mapper setup
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserDTO, User>();
        });
        _mapper = new Mapper(config);

        //Mockrepo setup
        _mockRepo = new Mock<IUserRepository>();
     
        _mockRepo.Setup(x => x.GetUserById(It.IsAny<int>())).Returns<int>(id => _fakeRepo.FirstOrDefault(p => p.Id == id));

        _mockRepo.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>(p =>
        {
            int index = _fakeRepo.FindIndex(c => c.Id == p.Id);
            if (index != -1)
            {
                _fakeRepo[index] = p;
            }
        });
        _userDtoValidator = new UserDTOValidator();

    }
    
    #region GetUserByIdTests
    
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
        var service = new UserService(_mockRepo.Object, _mapper, _userDtoValidator);
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
    public void GetUserById_InvalidId_Test()
    {
        // Arrange
        var id = 0;
        string error = "Id cannot be lower than 1";

        var service = new UserService(_mockRepo.Object, _mapper, _userDtoValidator);

        // Assert & act
        var ex = Assert.Throws<ArgumentException>(() => service.GetUserById(id));
        Assert.Equal(error, ex.Message);
        //_mockRepo.Verify(r => r.GetUserById(id), Times.Never);
    }
    
    [Fact]
    public void GetUserById_NonExistingPattern_Test()
    {
        // Arrange
        var id = 1;
        
        var service = new UserService(_mockRepo.Object, _mapper, _userDtoValidator);
        _mockRepo.Setup(r => r.GetUserById(id)).Returns(() => null);

        // Act
        var result = service.GetUserById(id);

        // Assert
        Assert.Null(result);
        _mockRepo.Verify(r => r.GetUserById(id), Times.Once);

    }

    #endregion //GetPatternById_Tests

    

 #region UpdatePostTests
    
        
    [Theory]

    [InlineData(1,"data:image/png;base64,filler", "filler", "11-11-2000", "email@live.dk")] // updates nothing
    [InlineData(1,"data:image/jpeg;base64,filler", "filler", "11-11-2000", "email@live.dk")] // updates profile pricture to jpeg
    [InlineData(1,"data:image/png;base64,filler", "test", "11-11-2000", "email@live.dk")] // updates username
    [InlineData(1,"data:image/png;base64,filler", "filler", "12-12-9999", "email@live.dk")] // updates birthday
    [InlineData(1,"data:image/png;base64,filler", "filler", "01-01-1000", "email@live.dk")] // updates birthday
    [InlineData(1,"data:image/png;base64,filler", "filler", "11-11-2000", "email123@live.dk")] // updates email
    [InlineData(1,null, "filler", "11-11-2000", "email@live.dk")] // updates with no image

    public void UpdateUser_Valid(int id, string profilePicture, string username, string birthdate, string email)
    {
        //arrange
        var  user = new UserDTO()
        {
            Id = 1,
            ProfilePicture = "data:image/png;base64,filler",
            Username = "filler",            
            BirthDay = DateOnly.Parse("11-11-2000"),
            Email = "email@live.dk",
        };
        _fakeRepo.Add(_mapper.Map<User>(user));

        var  updatedUser = new UserDTO()
        {
            Id = id,
            ProfilePicture = profilePicture,
            Email = email,
            BirthDay = DateOnly.Parse(birthdate),
            Username = username
        };

        //var repo = _mockRepo.Object;
        var service = new UserService(_mockRepo.Object, _mapper, _userDtoValidator);
        //act

        service.UpdateUser(updatedUser);
        var expected = JsonConvert.SerializeObject(_mapper.Map<User>(updatedUser));
        var actual = JsonConvert.SerializeObject(_fakeRepo[0]);
        
        
         //assert
        Assert.True(_fakeRepo.Count == 1);
        Assert.Equal(expected, actual);
        _mockRepo.Verify(r => r.UpdateUser(It.IsAny<User>()), Times.Once);        
    }

    [Fact]
    public void UpdateUser_Invalid_Null()
    {
        //arrange
        var repo = _mockRepo.Object;
        var service = new UserService(repo, _mapper, _userDtoValidator);

        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdateUser(null));
        Assert.Equal("User is null", ex.Message);
        _mockRepo.Verify(r => r.UpdateUser(null), Times.Never);
    }
    
    
    [Fact]
    public void UpdateUser_UserDoesNotExist_ExpectArgumentException_Test()
    {
        // Arrange
        var  user1 = new UserDTO()
        {
            Id = 1,
            ProfilePicture = "data:image/png;base64,filler",
            Username = "filler",            
            BirthDay = DateOnly.Parse("11-11-2000"),
            Email = "email@live.dk",
        };
        var  user2 = new UserDTO()
        {
            Id = 2,
            ProfilePicture = "data:image/png;base64,filler",
            Username = "filler",            
            BirthDay = DateOnly.Parse("11-11-2000"),
            Email = "email@live.dk",
        };
        _fakeRepo.Add(_mapper.Map<User>(user1));

        var repo = _mockRepo.Object;
        var service = new UserService(repo, _mapper, _userDtoValidator);

        // Act + Assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdateUser(user2));
        Assert.Equal("User id does not exist", ex.Message);
        _mockRepo.Verify(r => r.UpdateUser(_mapper.Map<User>(user2)), Times.Never);
    }
    
    [Theory]
    [InlineData(0,"data:image/png;base64,filler", "filler", "11-11-2000", "email@live.dk", "id cannot be below 1")] // id below 1
    [InlineData(int.MinValue,"data:image/png;base64,filler", "filler", "11-11-2000", "email@live.dk", "id cannot be below 1")] // id below 1
    [InlineData(1,"filler", "filler", "11-11-2000", "email@live.dk", "Only PNG and JPG files are allowed")] // base64 missing file type
    [InlineData(1,"data:application/pdf;base64,filler", "filler", "11-11-2000", "email@live.dk", "Only PNG and JPG files are allowed")] // base64 file type pdf 
    [InlineData(1,"", "filler", "11-11-2000", "email@live.dk", "Only PNG and JPG files are allowed")] // base64 string is empty 
    [InlineData(1,"data:image/png;base64,filler", "", "11-11-2000", "email@live.dk", "Username can not be empty or null")] // username is empty
    [InlineData(1,"data:image/png;base64,filler", null, "11-11-2000", "email@live.dk", "Username can not be empty or null")] //  username is null
    [InlineData(1,"data:image/png;base64,filler", "filler", "11-11-2000", "@live.dk", "Email has to be an Email")] // not  email
    [InlineData(1,"data:image/png;base64,filler", "filler", "11-11-2000", "email@", "Email has to be an Email")] //  not  email
    [InlineData(1,"data:image/png;base64,filler", "filler", "11-11-2000", "@", "Email has to be an Email")] //  not email
    [InlineData(1,"data:image/png;base64,filler", "filler", "11-11-2000", "", "Email has to be an Email")] //  not email
    [InlineData(1,"data:image/png;base64,filler", "filler", "11-11-2000", null, "Email has to be an Email")] // not email
    
    public void UpdateUser_invalid(int id, string profilePicture, string username, string birthdate, string email, string error)
    {
        //arrange
        var repo = _mockRepo.Object;
        var service = new UserService(repo, _mapper, _userDtoValidator);
        var  user = new UserDTO()
        {
            Id = id,
            ProfilePicture = profilePicture,
            Email = email,
            BirthDay = DateOnly.Parse(birthdate),
            Username = username
        };        
        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdateUser(user));
        Assert.Equal(error, ex.Message);
        _mockRepo.Verify(r => r.UpdateUser(It.IsAny<User>()), Times.Never); 
    }
    #endregion //UpdatePostTests

 
    
    
    
}