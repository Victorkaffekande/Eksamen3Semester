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
    private PatternDTOValidator _dtoValidator;
    private PatternValidator _patternValidator;
    
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
        _mockRepo.Setup(x => x.GetPatternById(It.IsAny<int>())).Returns<int>(id => fakeRepo.FirstOrDefault(x => x.Id == id));

        _mockRepo.Setup(x => x.UpdatePattern(It.IsAny<Pattern>())).Callback<Pattern>(p =>
        {
            int index = fakeRepo.FindIndex(c => c.Id == p.Id);
            if (index != -1)
            {
                fakeRepo[index] = p;
            }
        });
        _mockRepo.Setup(x => x.DeletePattern(It.IsAny<int>())).Callback<int>(s => fakeRepo.Remove(fakeRepo.Find(p => p.Id == s )));
        //Validator setup
        
        _dtoValidator = new PatternDTOValidator();
        _patternValidator = new PatternValidator();
        
    }

    #region Constructor tests

// Valid create test
    [Fact]
    public void CreatePatternService_Valid()
    {
        //arrange
        IPatternRepository repo = _mockRepo.Object;

        //act
        IPatternService service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);

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
        var ex = Assert.Throws<ArgumentException>(() => service = new PatternService(null, _mapper, _dtoValidator,_patternValidator));
        Assert.Equal("Missing Repository", ex.Message);
    }

    [Fact]
    public void CreatePatternService_NullMapper()
    {
        //Arrange
        IPatternRepository repo = _mockRepo.Object;
        PatternService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service = new PatternService(repo, null, _dtoValidator,_patternValidator));
        Assert.Equal("Missing Mapper", ex.Message);
    }

    [Fact]
    public void CreatePatternService_NullValidator()
    {
        //Arrange
        IPatternRepository repo = _mockRepo.Object;
        PatternService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service = new PatternService(repo, _mapper, null,_patternValidator));
        Assert.Equal("Missing Validator", ex.Message);
    }

    #endregion

    
    [Fact]
    public void CreatePattern_Valid()
    {
        //arrange
        var dto = new PatternDTO()
        {
            UserId = 1,
            Title = "fe",
            PdfString = "data:application/pdf;base64,wefwefewfwef",
            Image = "data:image/png;base64,wfwefwefw", Description = "Cool"
        };
        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);

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
        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);

        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.CreatePattern(null));
        Assert.Equal("PatternDTO is null", ex.Message);
        _mockRepo.Verify(r => r.CreatePattern(null), Times.Never);
    }
    
    [Theory]
    [InlineData(0,"title","data:application/pdf;base64,qweqweqw", "data:image/png;base64,qweq", "sdasd", "UserID must be higher than 0")]
    [InlineData(1,"","data:application/pdf;base64,qweqweqw", "data:image/png;base64,qweq", "sdasd", "title can not be empty or null")]
    [InlineData(1,null,"data:application/pdf;base64,qweqweqw", "data:image/png;base64,qweq", "sdasd", "title can not be empty or null")]
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
        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);
        PatternDTO dto = new PatternDTO()
        {
            UserId = id,
            Title = title,
            PdfString = pdf,
            Image = img,
            Description = desc
        };
        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.CreatePattern(dto));
        Assert.Equal(error, ex.Message);
        _mockRepo.Verify(r => r.CreatePattern(It.IsAny<Pattern>()), Times.Never); 
    }

    [Theory]
    [InlineData(1, "tittle", 1, "data:application/pdf;base64,filler","fillertext", "data:image/png;base64,filler")]  
    [InlineData(1, "tittle", 1, "data:application/pdf;base64,fillertest2","fillertext", "data:image/jpeg;base64,filler")]  
    public void UpdatePattern_Valid(int id, string title, int userid, string pdf, string desc, string image)
    {
        //arrange
        var  pattern = new Pattern()
        {
            Id = 1,
            Title = "filler",
            UserId = 1,
            PdfString = "data:application/pdf;base64,filler",
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };
        fakeRepo.Add(pattern);

        var  updatedPattern = new Pattern()
        {
            Id = id, 
            Title = title,
            UserId = userid,
            PdfString = pdf,
            Image = image,
            Description = desc
        };

        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);
        
        //act
        service.UpdatePattern(updatedPattern);
        
        //assert
        Assert.True(fakeRepo.Count == 1);
        Assert.Equal(updatedPattern, fakeRepo[0]);
        _mockRepo.Verify(r => r.UpdatePattern(updatedPattern), Times.Once);        
    }

    [Fact]
    public void UpdatePattern_Invalid_Null()
    {
        //arrange
        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);

        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdatePattern(null));
        Assert.Equal("Pattern is null", ex.Message);
        _mockRepo.Verify(r => r.CreatePattern(null), Times.Never);
    }
    
    [Fact]
    public void UpdateStudent_StudentDoesNotExist_ExpectArgumentException_Test()
    {
        // Arrange
        var  pattern = new Pattern()
        {
            Id = 1,
            Title = "filler",
            UserId = 1,
            PdfString = "data:application/pdf;base64,filler",
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };
        
        var  pattern2 = new Pattern()
        {
            Id = 2,
            Title = "filler",
            UserId = 1,
            PdfString = "data:application/pdf;base64,filler",
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };
        fakeRepo.Add(pattern);

        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);

        // Act + Assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdatePattern(pattern2));
        Assert.Equal("pattern id does not exist", ex.Message);
        _mockRepo.Verify(r => r.UpdatePattern(pattern2), Times.Never);
    }
    
    [Theory]
    [InlineData(1,0,"title","data:application/pdf;base64,qweqweqw", "data:image/png;base64,qweq", "sdasd", "UserID must be higher than 0")]
    [InlineData(0,1,"title","data:application/pdf;base64,qweqweqw", "data:image/png;base64,qweq", "sdasd", "ID must be higher than 0")]
    [InlineData(1,1,"","data:application/pdf;base64,qweqweqw", "data:image/png;base64,qweq", "sdasd", "title can not be empty or null")]
    [InlineData(1,1,null,"data:application/pdf;base64,qweqweqw", "data:image/png;base64,qweq", "sdasd", "title can not be empty or null")]
    [InlineData(1,1,"title","data:application/png;base64,qweqweqw", "data:image/png;base64,qweq", "sdasd", "this is a not pdf")]
    [InlineData(1,1,"title","", "data:application/png;base64,qweq", "sdasd", "this is a not pdf")]
    [InlineData(1,1,"title",null, "data:image/png;base64,qweq", "sdasd", "this is a not pdf")]
    [InlineData(1,1,"title","data:application/pdf;base64,qweqweqw", "data:image/pdf;base64,qweq", "sdasd", "this is not a png/jpeg")]
    [InlineData(1,1,"title","data:application/pdf;base64,qweqweqw", "", "sdasd", "this is not a png/jpeg")]
    [InlineData(1,1,"title","data:application/pdf;base64,qweqweqw", null, "sdasd", "this is not a png/jpeg")]
    [InlineData(1,1,"title","data:application/pdf;base64,qweqweqw", "data:image/png;base64,qweq", "", "Description can not be empty or null")]
    [InlineData(1,1,"title","data:application/pdf;base64,qweqweqw", "data:image/png;base64,qweq", null, "Description can not be empty or null")]

    public void UpdatePattern_invalid(int id, int userId, string title ,string pdf, string img, string desc, string error)
    {
        //arrange
        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);
        Pattern pattern = new Pattern() {Id = id, UserId = userId, Title = title, PdfString = pdf, Image = img, Description = desc };
        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdatePattern(pattern));
        Assert.Equal(error, ex.Message);
        _mockRepo.Verify(r => r.UpdatePattern(It.IsAny<Pattern>()), Times.Never); 
    }
    
    [Fact]
    public void GetPatternById_ExistingPattern_Test()
    {
        // Arrange
        var id = 1;
        Pattern existingPattern = new Pattern() {Id = id, UserId = 1, Title = "title", PdfString = "pdf", Image = "img", Description = "desc" };

        var service = new PatternService(_mockRepo.Object, _mapper, _dtoValidator,_patternValidator);
        _mockRepo.Setup(r => r.GetPatternById(id)).Returns(existingPattern);

        // Act
        var result = service.GetPatternById(id);

        // Assert
        Assert.Equal(existingPattern, result);
        _mockRepo.Verify(r => r.GetPatternById(id), Times.Once);
    }
    
    [Fact]
    public void GetPatternById_InvalidId_Test()
    {
        // Arrange
        var id = 0;
        string error = "Id cannot be lower than 1";

        var service = new PatternService(_mockRepo.Object, _mapper, _dtoValidator,_patternValidator);

        // Assert & act
        var ex = Assert.Throws<ArgumentException>(() => service.GetPatternById(id));
        Assert.Equal(error, ex.Message);
        _mockRepo.Verify(r => r.GetPatternById(id), Times.Never);
    }
    
    
    [Fact]
    public void GetPatternById_NonExistingStudent_Test()
    {
        // Arrange
        var id = 1;
        
        var service = new PatternService(_mockRepo.Object, _mapper, _dtoValidator,_patternValidator);
        _mockRepo.Setup(r => r.GetPatternById(id)).Returns(() => null);

        // Act
        var result = service.GetPatternById(id);

        // Assert
        Assert.Null(result);
        _mockRepo.Verify(r => r.GetPatternById(id), Times.Once);
    }
}
    
