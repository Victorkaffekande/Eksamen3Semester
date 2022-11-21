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
        _mockRepo.Setup(r => r.CreatePattern(It.IsAny<Pattern>())).Callback<Pattern>(p => fakeRepo.Add(p));
        
        _mockRepo.Setup(x => x.UpdatePattern(It.IsAny<Pattern>())).Callback<Pattern>(s =>
        {
            var index = fakeRepo.IndexOf(s);
            if (index != -1)
                fakeRepo[index] = s;
        });
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
        var dto = new PatternDTO(){UserId = 1, PdfString = "data:application/pdf;base64,wefwefewfwef", Image = "data:image/png;base64,wfwefwefw", Description = "Cool"};
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
    
    [Theory]
    [InlineData(0,"title","data:application/pdf;base64,qweqweqw", "data:image/png;base64,qweq", "sdasd", "UserID must be higher than 0")]
    [InlineData(0,"","data:application/pdf;base64,qweqweqw", "data:image/png;base64,qweq", "sdasd", "title can not be empty or null")]
    [InlineData(0,null,"data:application/pdf;base64,qweqweqw", "data:image/png;base64,qweq", "sdasd", "title can not be empty or null")]
    [InlineData(1,"title","data:application/png;base64,qweqweqw", "data:image/png;base64,qweq", "sdasd", "this is a not pdf")]
    [InlineData(1,"title","", "data:application/png;base64,qweq", "sdasd", "this is a not pdf")]
    [InlineData(1,"title",null, "data:image/png;base64,qweq", "sdasd", "this is a not pdf")]
    [InlineData(1,"title","data:application/pdf;base64,qweqweqw", "data:image/pdf;base64,qweq", "sdasd", "this is not a png/jpeg")]
    [InlineData(1,"title","data:application/pdf;base64,qweqweqw", "", "sdasd", "this is not a png/jpeg")]
    [InlineData(1,"title","data:application/pdf;base64,qweqweqw", null, "sdasd", "this is not a png/jpeg")]
    [InlineData(1,"title","data:application/pdf;base64,qweqweqw", "data:image/png;base64,qweq", "", "Description can not be empty or null")]
    [InlineData(1,"title","data:application/pdf;base64,qweqweqw", "data:image/png;base64,qweq", null, "Description can not be empty or null")]

    public void CreatePattern_invalid(int id, string title ,string pdf, string img, string desc, string error)
    {
        //arrange
        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _validator);
        PatternDTO dto = new PatternDTO() { UserId = id, Title = title, PdfString = pdf, Image = img, Description = desc };
        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.CreatePattern(dto));
        Assert.Equal(error, ex.Message);
        _mockRepo.Verify(r => r.CreatePattern(It.IsAny<Pattern>()), Times.Never); 
    }

    [Theory]
    [InlineData(1, "tittle", 1, "data:application/pdf;base64,filler","fillertext", "data:image/png;base64,filler")]  
    public void UpdatePattern_Valid(int id, string title, int userid, string pdf, string desc, string image)
    {
        //arrange
        var  pattern = new Pattern(){Id = 1, UserId = 1, PdfString = "data:application/pdf;base64,wefwefewfwef", Image = "data:application/png;base64,wfwefwefw", Description = "hej"};
        fakeRepo.Add(pattern);

        var  updatedPattern = new Pattern(){Id = id, Title = title, UserId = userid, PdfString = pdf, Image = image, Description = desc};

        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _validator);


        //act
        service.UpdatePattern(pattern);
        
        //assert
        Assert.True(fakeRepo.Count == 1);
        Assert.Equal(updatedPattern, fakeRepo[0]);
        _mockRepo.Verify(r => r.UpdatePattern(updatedPattern), Times.Once);        
    }

}
    
