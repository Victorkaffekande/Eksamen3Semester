using System.Net.Security;
using Application;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;
using Moq;

namespace UnitTestProject;

public class PatternServiceTest
{

    private List<Pattern> fakeRepo = new List<Pattern>();
    private Mapper _mapper;
    private Mock<IPatternRepository> _mockRepo;
    private PatternValidator _validator;

    //Constructor is run for every single test
    public PatternServiceTest()
    {
        //mapper setup
        var config = new MapperConfiguration(cfg => { cfg.CreateMap<PatternDTO, Pattern>(); });
        _mapper = new Mapper(config);

        //Mockrepo setup
        _mockRepo = new Mock<IPatternRepository>();
        _mockRepo.Setup(r => r.GetAllPattern()).Returns(fakeRepo);
        _mockRepo.Setup(r => r.CreatePattern(It.IsAny<Pattern>())).Callback<Pattern>(p => fakeRepo.Add(p)); //måske fucked

        //Validator setup
        _validator = new PatternValidator();
    }

    #region Constructor tests

// Valid create test
    [Fact]
    public void CreatePatternService_Valid()
    {
        //arrange
        IPatternRepository repo = _mockRepo.Object;

        //act
        IPatternService service = new PatternService(repo, _mapper, _validator);

        //assert
        Assert.NotNull(service);
        Assert.True(service is PatternService);
    }


    //Invalid create tests
    [Fact]
    public void CreatePatternService_NullRepo()
    {
        //Arrange
        PatternService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service = new PatternService(null, _mapper, _validator));
        Assert.Equal("Missing Repository", ex.Message);
    }

    [Fact]
    public void CreatePatternService_NullMapper()
    {
        //Arrange
        IPatternRepository repo = _mockRepo.Object;
        PatternService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service = new PatternService(repo, null, _validator));
        Assert.Equal("Missing Mapper", ex.Message);
    }

    [Fact]
    public void CreatePatternService_NullValidator()
    {
        //Arrange
        IPatternRepository repo = _mockRepo.Object;
        PatternService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service = new PatternService(repo, _mapper, null));
        Assert.Equal("Missing Validator", ex.Message);
    }

    #endregion

    
    [Fact]
    public void CreatePattern_Valid()
    {
        //arrange
        var user = new User() { Id = 1, Username = "test", Password = "test", Salt = "test", BirthDay = DateOnly.MaxValue };
        var dto = new PatternDTO(){UserId = 1, User = user, PdfString = "base64PDF", Image = "base65IMG", Description = "Cool"};
        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _validator);

        //act
        service.CreatePattern(dto);

        //assert
        Assert.NotNull(fakeRepo[0]);
        _mockRepo.Verify(r => r.CreatePattern(It.IsAny<Pattern>()), Times.Once);
    }

    
    
    
    [Fact]
    public void createPattern_Invalid_NullDto()
    {
        //arrange
        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _validator);

        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.CreatePattern(null));
        Assert.Equal("PatternDTO is null", ex.Message);
        _mockRepo.Verify(r => r.CreatePattern(null), Times.Never);
    }
    
    [MemberData(nameof(Memberdata_CreatePattern_invalid))]
    public void CreatePattern_invalid(List<PatternDTO> dto, List<string> expected)
    {
        //arrange
        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _validator);
        // act + assert
        Pattern p = _mapper.Map<Pattern>(dto);
        var ex = Assert.Throws<ArgumentException>(() => service.CreatePattern(dto.First()));
        Assert.Equal(expected.First(), ex.Message);
        _mockRepo.Verify(r => r.CreatePattern(p), Times.Never); // TODO ASK VICTOR IF THIS IS OKAY
    }

    public static IEnumerable<Object> Memberdata_CreatePattern_invalid()
    {
        //invalid create Pattern pdf cant be null 
        //TODO SHOULD WE ALSO TEST ON "" STRINGS?
        yield return new object[] 
        {
            new List<Object>(new[]
            {
                new PatternDTO(){UserId = 1, User = new User() { Id = 1, Username = "test", Password = "test", Salt = "test", BirthDay = DateOnly.MaxValue }, PdfString = null, Image = "base65IMG", Description = "Cool"}
            }),
            new List<string>(new[] { "No pdf selected" })
        };
        yield return new object[] 
        {
            new List<Object>(new[]
            {
                new PatternDTO(){UserId = 1, User = new User() { Id = 1, Username = "test", Password = "test", Salt = "test", BirthDay = DateOnly.MaxValue }, PdfString = "base64PDF", Description = "Cool"}
            }),
            new List<string>(new[] { "No image selected" })
        };
        yield return new object[] 
        {
            new List<Object>(new[]
            {
                new PatternDTO(){UserId = 1, User = new User() { Id = 1, Username = "test", Password = "test", Salt = "test", BirthDay = DateOnly.MaxValue }, PdfString = "base64PDF", Image = "base65IMG"}
            }),
            new List<string>(new[] { "Description is missing" })
        };
        yield return new object[] 
        {
            new List<Object>(new[]
            {
                new PatternDTO(){UserId = 1, PdfString = "base64PDF", Image = "base65IMG", Description = "Cool"}
            }),
            new List<string>(new[] { "No User" })
        };
        yield return new object[] 
        {
            new List<Object>(new[]
            {
                new PatternDTO(){ User = new User() { Id = 1, Username = "test", Password = "test", Salt = "test", BirthDay = DateOnly.MaxValue }, PdfString = "base64PDF", Image = "base65IMG", Description = "Cool"}
            }),
            new List<string>(new[] { "No User id" })
        };
    }

}
    
