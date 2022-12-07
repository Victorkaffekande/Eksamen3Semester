using Application;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;
using Moq;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace UnitTestProject;

public class PostServiceTest
{
    private List<Post> _fakeRepo = new List<Post>();
    private Mapper _mapper;
    private Mock<IPostRepository> _mockRepo;
    private PostCreateDTOValidator _postCreateDtoValidator;
    private PostUpdateValidator _postUpdateValidator;

    public PostServiceTest()
    {
        //mapper setup
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PostCreateDTO, Post>();
            cfg.CreateMap<PostUpdateDTO, Post>();
        });
        _mapper = new Mapper(config);

        //Mockrepo setup
        _mockRepo = new Mock<IPostRepository>();
        _mockRepo.Setup(r => r.CreatePost(It.IsAny<Post>())).Callback<Post>(p => _fakeRepo.Add(p));
        _mockRepo.Setup(x => x.GetPostGetById(It.IsAny<int>()))
            .Returns<int>(id => _fakeRepo.FirstOrDefault(p => p.Id == id));


        _mockRepo.Setup(x => x.UpdatePost(It.IsAny<Post>())).Callback<Post>(p =>
        {
            int index = _fakeRepo.FindIndex(c => c.Id == p.Id);
            if (index != -1)
            {
                _fakeRepo[index] = p;
            }
        });
        _mockRepo.Setup(x => x.DeletePost(It.IsAny<int>()))
            .Callback<int>(s => _fakeRepo.Remove(_fakeRepo.Find(p => p.Id == s)));
        //Validator setup
        _postCreateDtoValidator = new PostCreateDTOValidator();
        _postUpdateValidator = new PostUpdateValidator();
    }

    #region ConstructorTest

// Valid create test
    [Fact]
    public void CreatePostService_Valid()
    {
        //arrange
        IPostRepository repo = _mockRepo.Object;

        //act
        IPostService service = new PostService(repo, _mapper, _postCreateDtoValidator, _postUpdateValidator);

        //assert
        Assert.NotNull(service);
        Assert.True(service is PostService);
    }


    //Invalid create tests
    [Fact]
    public void CreatePostService_NullRepo()
    {
        //Arrange
        PostService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() =>
            service = new PostService(null, _mapper, _postCreateDtoValidator, _postUpdateValidator));
        Assert.Equal("Missing Repository", ex.Message);
    }

    [Fact]
    public void CreatePostService_NullMapper()
    {
        //Arrange
        IPostRepository repo = _mockRepo.Object;
        PostService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() =>
            service = new PostService(repo, null, _postCreateDtoValidator, _postUpdateValidator));
        Assert.Equal("Missing Mapper", ex.Message);
    }

    [Fact]
    public void CreatePostService_NullCreateValidator()
    {
        //Arrange
        IPostRepository repo = _mockRepo.Object;
        PostService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() =>
            service = new PostService(repo, _mapper, null, _postUpdateValidator));
        Assert.Equal("Missing Validator", ex.Message);
    }

    [Fact]
    public void CreatePostService_NullPostValidator()
    {
        //Arrange
        IPostRepository repo = _mockRepo.Object;
        PostService? service;

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() =>
            service = new PostService(repo, _mapper, _postCreateDtoValidator, null));
        Assert.Equal("Missing Validator", ex.Message);
    }

    #endregion // ConstructorTest

    #region CreatePostTest

    public void CreatePost_Valid()
    {
        //Arrange
        var dto = new PostCreateDTO()
        {
            ProjectId = 1,
            PostDate = DateTime.Now,
            Image = "data:image/png;base64,filler",
            Description = "test"
        };
        var repo = _mockRepo.Object;
        var service = new PostService(repo, _mapper, _postCreateDtoValidator, _postUpdateValidator);

        //act
        service.CreatePost(dto);

        //assert
        Assert.NotNull(_fakeRepo[0]);
        _mockRepo.Verify(r => r.CreatePost(It.IsAny<Post>()), Times.Once);
    }

    [Fact]
    public void createPost_Invalid_NullDto()
    {
        //arrange
        var repo = _mockRepo.Object;
        var service = new PostService(repo, _mapper, _postCreateDtoValidator, _postUpdateValidator);

        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.CreatePost(null));
        Assert.Equal("PostDTO is null", ex.Message);
        _mockRepo.Verify(r => r.CreatePost(null), Times.Never);
    }


    [Theory]
    [InlineData(0, "2000-11-11", "data:image/png;base64,filler", "filler", "Post must have a pattern Id")] // id is 0
    [InlineData(int.MinValue, "2000-11-11", "data:image/png;base64,filler", "filler",
        "Post must have a pattern Id")] // id is int min
    [InlineData(1, "2000-11-11", "data:image/pdf;base64,filler", "filler", "Only PNG and JPG files are allowed")] // 
    [InlineData(1, "2000-11-11", "", "filler", "Only PNG and JPG files are allowed")] // i
    [InlineData(1, "2000-11-11", null, "filler", "Image can not be null")] // 
    [InlineData(1, "2000-11-11", "data:image/png;base64,filler", "", "Description can not be empty or null")] //
    [InlineData(1, "2000-11-11", "data:image/png;base64,filler", null, "Description can not be empty or null")] //
    public void CreatePost_invalid(int id, string postDate, string img, string desc, string error)
    {
        //arrange
        var repo = _mockRepo.Object;
        var service = new PostService(repo, _mapper, _postCreateDtoValidator, _postUpdateValidator);
        PostCreateDTO dto = new PostCreateDTO()
        {
            ProjectId = id,
            PostDate = DateTime.Parse(postDate),
            Image = img,
            Description = desc
        };
        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.CreatePost(dto));
        Assert.Equal(error, ex.Message);
        _mockRepo.Verify(r => r.CreatePost(It.IsAny<Post>()), Times.Never);
    }

    #endregion //CreatePostTest

    #region UpdatePostTests

    [Theory]
    [InlineData(1, "data:image/png;base64,filler", "filler")] // updates nothing
    [InlineData(1, "data:image/png;base64,filler", "test")] // updates description
    [InlineData(1, "data:image/jpeg;base64,filler", "filler")] // updates image 
    public void UpdatePost_Valid(int id, string img, string desc)
    {
        //arrange
        var post = new PostUpdateDTO()
        {
            Id = 1,
            Image = "data:image/png;base64,filler",
            Description = "filler"
        };
        _fakeRepo.Add(_mapper.Map<Post>(post));

        var updatedPost = new PostUpdateDTO()
        {
            Id = id,
            Image = img,
            Description = desc
        };

        //var repo = _mockRepo.Object;
        var service = new PostService(_mockRepo.Object, _mapper, _postCreateDtoValidator, _postUpdateValidator);
        //act

        service.UpdatePost(updatedPost);
        var expected = JsonConvert.SerializeObject(_mapper.Map<Post>(updatedPost));
        var actual = JsonConvert.SerializeObject(_mapper.Map<Post>(_fakeRepo[0]));


        //assert
        Assert.True(_fakeRepo.Count == 1);
        Assert.Equal(expected, actual);
        _mockRepo.Verify(r => r.UpdatePost(It.IsAny<Post>()), Times.Once);
    }

    [Fact]
    public void UpdatePost_Invalid_Null()
    {
        //arrange
        var repo = _mockRepo.Object;
        var service = new PostService(repo, _mapper, _postCreateDtoValidator, _postUpdateValidator);

        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdatePost(null));
        Assert.Equal("Post is null", ex.Message);
        _mockRepo.Verify(r => r.UpdatePost(null), Times.Never);
    }


    [Fact]
    public void UpdatePost_PostDoesNotExist_ExpectArgumentException_Test()
    {
        // Arrange
        var post1 = new PostUpdateDTO()
        {
            Id = 1,
            Image = "data:image/png;base64,filler",
            Description = "filler"
        };

        var post2 = new PostUpdateDTO()
        {
            Id = 2,
            Image = "data:image/png;base64,filler",
            Description = "filler"
        };
        _fakeRepo.Add(_mapper.Map<Post>(post1));

        var repo = _mockRepo.Object;
        var service = new PostService(repo, _mapper, _postCreateDtoValidator, _postUpdateValidator);

        // Act + Assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdatePost(post2));
        Assert.Equal("Post id does not exist", ex.Message);
        _mockRepo.Verify(r => r.UpdatePost(_mapper.Map<Post>(post2)), Times.Never);
    }

    [Theory]
    [InlineData(0, "data:image/png;base64,filler", "filler", "Post id must be 1 or higher")] // id is 0
    [InlineData(int.MinValue, "data:image/png;base64,filler", "filler", "Post id must be 1 or higher")] // id is int min
    [InlineData(1, null, "filler", "Image can not be null")] // image is null
    [InlineData(1, "", "filler", "Only PNG and JPG files are allowed")] // 
    [InlineData(1, "data:image/pdf;base64,filler", "filler", "Only PNG and JPG files are allowed")] // 
    [InlineData(1, "data:image/png;base64,filler", "", "Description can not be empty or null")] // 
    [InlineData(1, "data:image/png;base64,filler", null, "Description can not be empty or null")] // 
    public void UpdatePost_invalid(int id, string image, string description, string error)
    {
        //arrange
        var repo = _mockRepo.Object;
        var service = new PostService(repo, _mapper, _postCreateDtoValidator, _postUpdateValidator);
        PostUpdateDTO post = new PostUpdateDTO() { Id = id, Description = description, Image = image, };
        // act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.UpdatePost(post));
        Assert.Equal(error, ex.Message);
        _mockRepo.Verify(r => r.UpdatePost(It.IsAny<Post>()), Times.Never);
    }

    #endregion //UpdatePostTests

    #region DeletePostTests

    public void DeletePost_ValidPost_Test()
    {
        // Arrange
        var post1 = new Post()
        {
            Id = 1,
            ProjectId = 1,
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };
        var post2 = new Post()
        {
            Id = 2,
            ProjectId = 1,
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };
        _fakeRepo.Add(post1);
        _fakeRepo.Add(post2);

        var repo = _mockRepo.Object;
        var service = new PostService(repo, _mapper, _postCreateDtoValidator, _postUpdateValidator);
        // Act
        service.DeletePost(1);

        // Assert
        Assert.True(_fakeRepo.Count == 1);
        Assert.Contains(post2, _fakeRepo);
        Assert.DoesNotContain(post1, _fakeRepo);
        _mockRepo.Verify(r => r.DeletePost(1), Times.Once);
    }

    [Fact]
    public void DeletePost_IdInvalid_Test()
    {
        // Arrange
        var repo = _mockRepo.Object;

        var service = new PostService(repo, _mapper, _postCreateDtoValidator, _postUpdateValidator);

        // Act and assert
        var ex = Assert.Throws<ArgumentException>(() => service.DeletePost(0));
        Assert.Equal("id cannot be under 1", ex.Message);
        _mockRepo.Verify(r => r.DeletePost(0), Times.Never);
    }

    [Fact]
    public void DeletePost_PostDoesNotExist_ExpectArgumentException()
    {
        // Arrange
        var post = new Post()
        {
            Id = 1,
            ProjectId = 1,
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };

        _fakeRepo.Add(post);
        var repo = _mockRepo.Object;

        var service = new PostService(repo, _mapper, _postCreateDtoValidator, _postUpdateValidator);

        // Act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.DeletePost(2));
        Assert.Equal("Post does not exist", ex.Message);
        Assert.Contains(post, _fakeRepo);
        _mockRepo.Verify(r => r.DeletePost(2), Times.Never);
    }

    #endregion //DeletePostTests

    #region GetPostByIdTests

    [Fact]
    public void GetPostById_ExistingPost_Test()
    {
        // Arrange
        var id = 1;
        Post existingPost = new Post() { Id = id, ProjectId = 1, Image = "img", Description = "desc" };

        var service = new PostService(_mockRepo.Object, _mapper, _postCreateDtoValidator, _postUpdateValidator);
        _mockRepo.Setup(r => r.GetPostGetById(id)).Returns(existingPost);

        // Act
        var result = service.GetPostById(id);

        // Assert
        Assert.Equal(existingPost, result);
        _mockRepo.Verify(r => r.GetPostGetById(id), Times.Once);
    }

    public void GetPostById_InvalidId_Test()
    {
        // Arrange
        var id = 0;
        string error = "Id cannot be lower than 1";

        var service = new PostService(_mockRepo.Object, _mapper, _postCreateDtoValidator, _postUpdateValidator);

        // Assert & act
        var ex = Assert.Throws<ArgumentException>(() => service.GetPostById(id));
        Assert.Equal(error, ex.Message);
        _mockRepo.Verify(r => r.GetPostGetById(id), Times.Never);
    }

    [Fact]
    public void GetPostById_NonExistingPost_Test()
    {
        // Arrange
        var id = 1;

        var service = new PostService(_mockRepo.Object, _mapper, _postCreateDtoValidator, _postUpdateValidator);
        _mockRepo.Setup(r => r.GetPostGetById(id)).Returns(() => null);

        // Act
        var result = service.GetPostById(id);

        // Assert
        Assert.Null(result);
        _mockRepo.Verify(r => r.GetPostGetById(id), Times.Once);
    }

    #endregion //GetPostByIdTest
    
    #region GetAllPostByProjectTest

    [Fact]
    public void GetAllPostByProject_Test()
    {
        // Arrange
        var post1 = new Post()
        {
            Id = 1,
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };
        var post2 = new Post()
        {
            Id = 2,
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };
        var post3 = new Post()
        {
            Id = 3,
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };

        var expectedPost1 = new PostFromProjectDTO()
        {
            Id = 1,
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };

        var expectedPost2 = new PostFromProjectDTO()
        {
            Id = 2,
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };
        var expectedPost3 = new PostFromProjectDTO()
        {
            Id = 3,
            Image = "data:image/png;base64,filler",
            Description = "hej"
        };
        
        _mockRepo.Setup(x => x.GetAllPostFromProject(1))
            .Returns(() => _fakeRepo.FindAll(p => p.Id < 3));

        _fakeRepo.Add(post1);
        _fakeRepo.Add(post2);
        _fakeRepo.Add(post3);
        
        var repo = _mockRepo.Object;
        var service = new PostService(repo, _mapper, _postCreateDtoValidator, _postUpdateValidator);

        // Act
        var result = service.GetAllPostFromProject(1).ToList();
        var jsonResult = JsonConvert.SerializeObject(result);
        
        //assert
        Assert.True(result.Count == 2);
        Assert.Contains(JsonSerializer.Serialize(expectedPost1), jsonResult);
        Assert.Contains(JsonSerializer.Serialize(expectedPost2), jsonResult);
  
        Assert.DoesNotContain(JsonSerializer.Serialize(expectedPost3),jsonResult);
        _mockRepo.Verify(r => r.GetAllPostFromProject(1), Times.Once);
    }

    [Fact]
    public void GetPostByProjectId_InvalidId_Test()
    {
        // Arrange
        var id = 0;
        string error = "Id cannot be lower than 1";

        var service = new PostService(_mockRepo.Object, _mapper, _postCreateDtoValidator, _postUpdateValidator);

        // Assert & act
        var ex = Assert.Throws<ArgumentException>(() => service.GetAllPostFromProject(id));
        Assert.Equal(error, ex.Message);
        _mockRepo.Verify(r => r.GetAllPostFromProject(id), Times.Never);
    }

    #endregion //GetAllPostByProjectTest
}