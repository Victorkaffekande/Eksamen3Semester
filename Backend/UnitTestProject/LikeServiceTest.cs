using System.Collections;
using Application;
using Application.DTOs.Like;
using Application.Interfaces.Like_Interfaces;
using AutoMapper;
using Domain;
using Moq;

namespace UnitTestProject;

public class LikeServiceTest
{
    private List<Like> fakeRepo = new List<Like>();
    private Mock<ILikeRepository> _mockRepo;
    private Mapper _mapper;

    public LikeServiceTest()
    {
        _mockRepo = new Mock<ILikeRepository>();
        _mockRepo.Setup(r => r.LikeUser(It.IsAny<Like>()))
            .Callback<Like>(l => fakeRepo.Add(l));
        _mockRepo.Setup(r => r.RemoveLike(It.IsAny<Like>()))
            .Callback<Like>(x =>
                fakeRepo.Remove(fakeRepo.Find(l => l.UserId == x.UserId && l.LikedUserId == x.LikedUserId)));


        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SimpleLikeDto, Like>();
            cfg.CreateMap<Like, SimpleLikeDto>();
        });
        _mapper = new Mapper(config);
    }

    #region Constructor tests

    [Fact]
    public void Constructor_Valid()
    {
        var repo = _mockRepo.Object;
        ILikeService service = new LikeService(repo, _mapper);

        Assert.NotNull(service);
        Assert.True(service is LikeService);
    }

    [Fact]
    public void Constructor_Invalid_RepoNull()
    {
        ILikeService? service;

        var ex = Assert.Throws<ArgumentException>(() =>
            service = new LikeService(null, _mapper));
        Assert.Equal("Missing repository", ex.Message);
    }

    [Fact]
    public void Constructor_Invalid_MapperNull()
    {
        var repo = _mockRepo.Object;
        ILikeService? service;

        var ex = Assert.Throws<ArgumentException>(() =>
            service = new LikeService(repo, null));
        Assert.Equal("Missing mapper", ex.Message);
    }

    #endregion

    #region LikeUser tests

    [Fact]
    public void LikeUser_valid()
    {
        //arrange
        var likeDto = new SimpleLikeDto()
        {
            UserId = 1,
            LikedUserId = 2
        };
        var repo = _mockRepo.Object;
        ILikeService service = new LikeService(repo, _mapper);


        //act
        service.LikeUser(likeDto);

        //assert
        Assert.NotNull(fakeRepo[0]);
        Assert.True(fakeRepo.Count is 1);
        _mockRepo.Verify(r => r.LikeUser(It.IsAny<Like>()), Times.Once());
    }


    [Fact]
    public void LikeUser_Invalid_NullDto()
    {
        //arrange
        var repo = _mockRepo.Object;
        ILikeService service = new LikeService(repo, _mapper);

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.LikeUser(null));
        Assert.Equal("SimpleLikeDto is null", ex.Message);
        _mockRepo.Verify(r => r.LikeUser(It.IsAny<Like>()), Times.Never);
    }

    [Fact]
    public void LikeUser_Invalid_AlReadyLikes()
    {
        //arrange
        var likeDto = new SimpleLikeDto()
        {
            UserId = 1,
            LikedUserId = 2
        };
        var repo = _mockRepo.Object;
        ILikeService service = new LikeService(repo, _mapper);
        _mockRepo.Setup(r => r.AlreadyLikes(It.IsAny<Like>())).Returns(_mapper.Map<Like>(likeDto));
        fakeRepo.Add(_mapper.Map<Like>(likeDto));

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.LikeUser(likeDto));
        Assert.Equal("Like already exists", ex.Message);
        _mockRepo.Verify(r => r.LikeUser(It.IsAny<Like>()), Times.Never);
    }


    [Theory]
    [InlineData(5, 1)]
    [InlineData(1, 5)]
    public void LikeUser_Invalid_UserIdDoesNotExist(int userId, int likedUserId)
    {
        //arrange
        var repo = _mockRepo.Object;
        ILikeService service = new LikeService(repo, _mapper);
        var likeDto = new SimpleLikeDto()
        {
            UserId = userId,
            LikedUserId = likedUserId
        };
        _mockRepo.Setup(r => r.DoesUserExist(likeDto)).Returns(true);

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.LikeUser(likeDto));
        Assert.Equal("One or more userIds don't exist", ex.Message);
        _mockRepo.Verify(r => r.LikeUser(It.IsAny<Like>()), Times.Never);
    }

    #endregion

    #region RemoveLike tests

    //valid remove
    [Fact]
    public void RemoveLike_Valid_LikeExists()
    {
        //arrange
        var repo = _mockRepo.Object;
        ILikeService service = new LikeService(repo, _mapper);
        var dto1 = new SimpleLikeDto()
        {
            UserId = 1, LikedUserId = 2
        };
        var dto2 = new SimpleLikeDto()
        {
            UserId = 1, LikedUserId = 3
        };
        fakeRepo.Add(_mapper.Map<Like>(dto1));
        fakeRepo.Add(_mapper.Map<Like>(dto2));

        //act
        service.RemoveLike(dto1);

        //assert
        Assert.True(fakeRepo.Count is 1);
        _mockRepo.Verify(r => r.RemoveLike(It.IsAny<Like>()), Times.Once);
    }

    [Fact]
    public void RemoveLike_Valid_LikeDoesNotExists()
    {
        //arrange
        var repo = _mockRepo.Object;
        ILikeService service = new LikeService(repo, _mapper);
        var dto1 = new SimpleLikeDto()
        {
            UserId = 1, LikedUserId = 2
        };
        var dto2 = new SimpleLikeDto()
        {
            UserId = 1, LikedUserId = 3
        };
        fakeRepo.Add(_mapper.Map<Like>(dto1));

        //act
        service.RemoveLike(dto2);

        //assert
        Assert.True(fakeRepo.Count is 1);
        _mockRepo.Verify(r => r.RemoveLike(It.IsAny<Like>()), Times.Once);
    }

    [Fact]
    public void RemoveLike_Invalid_InputIsNull()
    {
        //arrange
        var repo = _mockRepo.Object;
        ILikeService service = new LikeService(repo, _mapper);

        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.RemoveLike(null));
        Assert.Equal("Input is null", ex.Message);
        _mockRepo.Verify(r => r.RemoveLike(It.IsAny<Like>()), Times.Never);
    }

    #endregion
}