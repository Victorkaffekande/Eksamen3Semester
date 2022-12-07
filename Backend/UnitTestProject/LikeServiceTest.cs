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


        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SimpleLikeDto, Like>();
            cfg.CreateMap<Like, SimpleLikeDto>();
        });
        _mapper = new Mapper(config);
    }

    #region MyRegion

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

    //allready exists
    //invalid user ids
    [Fact]
    public void LikeUser_Invalid_AlReadyLikes()
    {
        
        //TODO WORK IN PROGRESS
        //arrange
        var likeDto = new SimpleLikeDto()
        {
            UserId = 1,
            LikedUserId = 2
        };
        var repo = _mockRepo.Object;
        ILikeService service = new LikeService(repo, _mapper);
        fakeRepo.Add(_mapper.Map<Like>(likeDto));
        
        //act + assert
        var ex = Assert.Throws<ArgumentException>(() => service.LikeUser(likeDto));
        Assert.Equal("Like already exists", ex.Message);
    }

    #endregion
}