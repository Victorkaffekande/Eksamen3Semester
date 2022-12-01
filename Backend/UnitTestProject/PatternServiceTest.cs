using System.Net.Security;
using Application;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;
using Moq;
using Newtonsoft.Json;

namespace UnitTestProject;

public class PatternServiceTest
{

    private List<Pattern> _fakeRepo = new List<Pattern>();
    private Mapper _mapper;
    private Mock<IPatternRepository> _mockRepo;
    private PatternDTOValidator _dtoValidator;
    private PatternValidator _patternValidator;
    
    //Constructor is run for every single test
    public PatternServiceTest()
    {
        //mapper setup
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PatternDTO, Pattern>();
            cfg.CreateMap<PatternUpdateDTO, Pattern>();
            
        });
        _mapper = new Mapper(config);

        //Mockrepo setup
        _mockRepo = new Mock<IPatternRepository>();
        _mockRepo.Setup(r => r.GetAllPattern()).Returns(_fakeRepo);
        _mockRepo.Setup(r => r.CreatePattern(It.IsAny<Pattern>())).Callback<Pattern>(p => _fakeRepo.Add(p));
        _mockRepo.Setup(x => x.GetPatternById(It.IsAny<int>())).Returns<int>(id => _fakeRepo.FirstOrDefault(p => p.Id == id));
        _mockRepo.Setup(x => x.GetAllPatternsByUser(It.IsAny<int>())).Returns<int>(id => _fakeRepo.FindAll(p => p.UserId == id));

        _mockRepo.Setup(x => x.UpdatePattern(It.IsAny<Pattern>())).Callback<Pattern>(p =>
        {
            int index = _fakeRepo.FindIndex(c => c.Id == p.Id);
            if (index != -1)
            {
                _fakeRepo[index] = p;
            }
        });
        _mockRepo.Setup(x => x.DeletePattern(It.IsAny<int>())).Callback<int>(s => _fakeRepo.Remove(_fakeRepo.Find(p => p.Id == s )));
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

    #region CreatePattern_Tests
    
    [Fact]
    public void CreatePattern_Valid()
    {
        //Arrange
        var dto = new PatternDTO()
        {
            UserId = 1,
            Title = "test",
            PdfString = "data:application/pdf;base64,filler",
            Image = "data:image/png;base64,filler",
            Description = "test"
        };
        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);

        //act
        service.CreatePattern(dto);

        //assert
        Assert.NotNull(_fakeRepo[0]);
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
    [InlineData(0,"title","data:application/pdf;base64,filler", "data:image/png;base64,filler", "filler", "UserID must be higher than 0")] // id is 0
    [InlineData(1,"","data:application/pdf;base64,filler", "data:image/png;base64,filler", "filler", "title can not be empty or null")] // title is empty
    [InlineData(1,null,"data:application/pdf;base64,filler", "data:image/png;base64,filler", "filler", "title can not be empty or null")] // title is null
    [InlineData(1,"title","data:application/png;base64,filler", "data:image/png;base64,filler", "filler", "this is a not pdf")] // blob not configured as pdf
    [InlineData(1,"title","", "data:application/png;base64,filler", "filler", "this is a not pdf")] // pdf blob is empty
    [InlineData(1,"title",null, "data:image/png;base64,filler", "filler", "this is a not pdf")] // pdf blob is null
    [InlineData(1,"title","data:application/pdf;base64,filler", "data:image/pdf;base64,filler", "filler", "this is not a png/jpeg")] // image blob is not configured right
    [InlineData(1,"title","data:application/pdf;base64,filler", "", "filler", "this is not a png/jpeg")] // image blob is empty
    [InlineData(1,"title","data:application/pdf;base64,filler", null, "filler", "this is not a png/jpeg")] // image blob is null
    [InlineData(1,"title","data:application/pdf;base64,filler", "data:image/png;base64,filler", "", "Description can not be empty or null")] // Description is empty
    [InlineData(1,"title","data:application/pdf;base64,filler", "data:image/png;base64,filler", null, "Description can not be empty or null")] // Description is null

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
    #endregion CreatePattern_Tests

    #region UpdatePattern_Tests

    

    
    [Theory]
    [InlineData(1,1, "test",  "data:application/pdf;base64,filler","filler", "data:image/png;base64,filler")]  // update tittle
    [InlineData(1,1, "filler",  "data:application/pdf;base64,test","filler", "data:image/png;base64,filler")]  // update pdf
    [InlineData(1,1, "filler",  "data:application/pdf;base64,filler","test", "data:image/png;base64,filler")]  // update description 
    [InlineData(1,1, "filler",  "data:application/pdf;base64,filler","filler", "data:image/jpeg;base64,filler")]  //updates image
    [InlineData(1,1, "filler", "data:application/pdf;base64,filler","filler", "data:image/png;base64,filler")] //update nothing 
    [InlineData(1,2, "filler", "data:application/pdf;base64,filler","filler", "data:image/png;base64,filler")] //update nothing 
    
    public void UpdatePattern_Valid(int id, int userId, string title, string pdf, string desc, string image)
    {
        //arrange
        var  pattern = new PatternUpdateDTO()
        {
            Id = 1,
            Title = "filler",
            UserId = 1,
            PdfString = "data:application/pdf;base64,filler",
            Image = "data:image/png;base64,filler",
            Description = "filler"
        };
        _fakeRepo.Add(_mapper.Map<Pattern>(pattern));

        var  updatedPattern = new PatternUpdateDTO() 
        {
            Id = id,
            Title = title,
            UserId = userId,
            PdfString = pdf,
            Image = image,
            Description = desc
        };

        //var repo = _mockRepo.Object;
        var service = new PatternService(_mockRepo.Object, _mapper, _dtoValidator,_patternValidator);
        //act

        service.UpdatePattern(updatedPattern);
        var expected = JsonConvert.SerializeObject(_mapper.Map<Pattern>(updatedPattern));
        var actual = JsonConvert.SerializeObject(_mapper.Map<Pattern>(_fakeRepo[0]));
        
        
         //assert
        Assert.True(_fakeRepo.Count == 1);
        Assert.Equal(expected, actual);
        _mockRepo.Verify(r => r.UpdatePattern(It.IsAny<Pattern>()), Times.Once);        
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
        _mockRepo.Verify(r => r.UpdatePattern(null), Times.Never);
    }
    
    [Fact]
    public void UpdatePattern_PatternDoesNotExist_ExpectArgumentException_Test()
    {
        // Arrange
        var  pattern = new PatternUpdateDTO()
        {
            Id = 1,
            Title = "filler",
            UserId = 1,
            PdfString = "data:application/pdf;base64,filler",
            Image = "data:image/png;base64,filler",
            Description = "filler"
        };
        
        var  pattern2 = new PatternUpdateDTO()
        {
            Id = 2,
            Title = "filler",
            UserId = 1,
            PdfString = "data:application/pdf;base64,filler",
            Image = "data:image/png;base64,filler",
            Description = "filler"
        };
        _fakeRepo.Add(_mapper.Map<Pattern>(pattern));

        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);

        // Act + Assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdatePattern(pattern2));
        Assert.Equal("pattern id does not exist", ex.Message);
        _mockRepo.Verify(r => r.UpdatePattern(_mapper.Map<Pattern>(pattern2)), Times.Never);
    }
    
    [Theory]
    [InlineData(0,1,"title","data:application/pdf;base64,filler", "data:image/png;base64,filler", "filler", "ID must be higher than 0")]  // id is 0
    [InlineData(1,1,"","data:application/pdf;base64,filler", "data:image/png;base64,filler", "filler", "title can not be empty or null")] // title is empty
    [InlineData(1,1,null,"data:application/pdf;base64,filler", "data:image/png;base64,filler", "filler", "title can not be empty or null")] // title is null
    [InlineData(1,1,"title","data:application/png;base64,filler", "data:image/png;base64,filler", "filler", "this is a not pdf")] // pdf blob is configured wrong
    [InlineData(1,1,"title","", "data:application/png;base64,filler", "filler", "this is a not pdf")] // pdf blob is empty
    [InlineData(1,1,"title",null, "data:image/png;base64,filler", "filler", "this is a not pdf")] // pdf blob is null
    [InlineData(1,1,"title","data:application/pdf;base64,filler", "data:image/pdf;base64,filler", "filler", "this is not a png/jpeg")] // image blob is configured wrong
    [InlineData(1,1,"title","data:application/pdf;base64,filler", "", "filler", "this is not a png/jpeg")] // image blob is empty
    [InlineData(1,1,"title","data:application/pdf;base64,filler", null, "filler", "this is not a png/jpeg")] // image blob is null
    [InlineData(1,1,"title","data:application/pdf;base64,filler", "data:image/png;base64,filler", "", "Description can not be empty or null")] //description is empty
    [InlineData(1,1,"title","data:application/pdf;base64,filler", "data:image/png;base64,filler", null, "Description can not be empty or null")] // description is null

    public void UpdatePattern_invalid(int id, int userId, string title ,string pdf, string img, string desc, string error)
    {
        //arrange
        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);
        PatternUpdateDTO pattern = new PatternUpdateDTO() {Id = id, UserId = userId, Title = title, PdfString = pdf, Image = img, Description = desc };
        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdatePattern(pattern));
        Assert.Equal(error, ex.Message);
        _mockRepo.Verify(r => r.UpdatePattern(It.IsAny<Pattern>()), Times.Never); 
    }
    #endregion UpdatePattern_Tests

    
    
    
    #region RemovePatternTests
    [Fact]
    public void RemovePattern_ValidPattern_Test()
    {
        // Arrange
        var  pattern1 = new Pattern()
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
        _fakeRepo.Add(pattern1);
        _fakeRepo.Add(pattern2);

        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);
        // Act
        service.DeletePattern(1);

        // Assert
        Assert.True(_fakeRepo.Count == 1);
        Assert.Contains(pattern2, _fakeRepo);
        Assert.DoesNotContain(pattern1, _fakeRepo);
        _mockRepo.Verify(r => r.DeletePattern(1), Times.Once);
    }
    [Fact]
    public void RemovePattern_IdInvalid_Test()
    {
        // Arrange
        var repo = _mockRepo.Object;

        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);

        // Act and assert
        var ex = Assert.Throws<ArgumentException>(() => service.DeletePattern(0));
        Assert.Equal("id cannot be under 1", ex.Message);
        _mockRepo.Verify(r => r.DeletePattern(0), Times.Never);
    }
    
    [Fact]
    public void RemovePattern_PatternDoesNotExist_ExpectArgumentException()
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
        
      
        _fakeRepo.Add(pattern);
        var repo = _mockRepo.Object;

        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);
        
        // Act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.DeletePattern(2));
        Assert.Equal("Pattern does not exist", ex.Message);
        Assert.Contains(pattern, _fakeRepo);
        _mockRepo.Verify(r => r.DeletePattern(2), Times.Never);
    }
    #endregion //RemovePatternTests

    #region GetPatternByIdTests
    
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
    public void GetPatternById_NonExistingPattern_Test()
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

    #endregion //GetPatternById_Tests
    
    #region GetAllPatternsTests

    [Fact]
    public void GetAllPatterns_Test()
    {
        // Arrange
        
        var  pattern1 = new Pattern()
        {
            Id = 1,
            Title = "filler",
            UserId = 1,
            User = new User(),
            PdfString = "data:application/pdf;base64,filler",
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };
        var patternDTO1 = new PatternGetAllDTO()
        {
            Id = 1,
            Title = "filler",
            User = new UserDTO(),
            Image = "data:image/png;base64,filler",
            
        };
        var  pattern2 = new Pattern()
        {
            Id = 2,
            Title = "filler",
            UserId = 1,
            PdfString = "data:application/pdf;base64,filler",
            Image = "data:image/png;base64,filler",
            User = new User(),
            Description = "hej"
        };
        var patternDTO2 = new PatternGetAllDTO()
        {
            Id = 2,
            Title = "filler",
            Image = "data:image/png;base64,filler",
            User = new UserDTO(),
        };
        _fakeRepo.Add(pattern1);
        _fakeRepo.Add(pattern2);

        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);

        // assert & act
        var result = service.GetAllPattern().ToList();

        var expected1 = JsonConvert.SerializeObject(patternDTO1);
        var actual1 = JsonConvert.SerializeObject(result[0]);
        
        var expected2 = JsonConvert.SerializeObject(patternDTO2);
        var actual2 = JsonConvert.SerializeObject(result[1]);
        
        Assert.True(result.Count == 2);
        Assert.Equal(expected1, actual1);
        Assert.Equal(expected2, actual2);
        _mockRepo.Verify(r => r.GetAllPattern(), Times.Once);
    }

    #endregion // GetAllPatternsTest
    
    #region GetAllPatternsByUser_Tests

    [Fact]
    public void GetAllPatternByUser_Test()
    {
        // Arrange
        // Arrange
        var  pattern1 = new Pattern()
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
            UserId = 2,
            PdfString = "data:application/pdf;base64,filler",
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };
        var  pattern3 = new Pattern()
        {
            Id = 3,
            Title = "filler",
            UserId = 1,
            PdfString = "data:application/pdf;base64,filler",
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };
        _fakeRepo.Add(pattern1);
        _fakeRepo.Add(pattern2);
        _fakeRepo.Add(pattern3);

        var repo = _mockRepo.Object;
        var service = new PatternService(repo, _mapper, _dtoValidator,_patternValidator);
        
        
        // Act
        var result = service.GetAllPatternsByUser(1).ToList();
        Assert.True(result.Count == 2);
        
        Assert.Contains(pattern1, result);
        Assert.Contains(pattern3, result);
        Assert.DoesNotContain(pattern2,result);
        _mockRepo.Verify(r => r.GetAllPatternsByUser(1), Times.Once);
    }
    
    
    [Fact]
    public void GetPatternByUser_InvalidId_Test()
    {
        // Arrange
        var id = 0;
        string error = "Id cannot be lower than 1";

        var service = new PatternService(_mockRepo.Object, _mapper, _dtoValidator,_patternValidator);

        // Assert & act
        var ex = Assert.Throws<ArgumentException>(() => service.GetAllPatternsByUser(id));
        Assert.Equal(error, ex.Message);
        _mockRepo.Verify(r => r.GetAllPatternsByUser(id), Times.Never);
    }
    
    [Fact]
    public void GetPatternByUser_NonExistingPattern_Test()
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
    
    #endregion // GetAllPatternsByUser_Tests
}
    