﻿using Application.DTOs.Like;
using Application.Interfaces.Like_Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LikeController : ControllerBase
{

    private ILikeService _service;

    public LikeController(ILikeService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("LikeUser")]
    public ActionResult LikeUser([FromBody] SimpleLikeDto dto)
    {
        try
        {
            return Ok(_service.LikeUser(dto));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Route("RemoveLike")]
    public ActionResult RemoveLike([FromBody] SimpleLikeDto dto)
    {
        try
        {
            return Ok(_service.RemoveLike(dto));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
    [HttpPost]
    [Route("AlreadyLikes")]
    public ActionResult<Boolean> AlreadyLikes([FromBody] SimpleLikeDto dto)
    {
        try
        {
            return Ok(_service.AlreadyLike(dto));

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("GetLikedUsers/{id}")]
    public ActionResult GetAllLikedUsersByUser([FromRoute] int id)
    {
        try
        {
            return Ok(_service.GetAllLikedUsersByUser(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("GetLikedUsersPost/{id}")]
    public ActionResult GetallPostByLikedUsersByUser([FromRoute] int id)
    {
        try
        {
            return Ok(_service.GetallPostByLikedUsersByUser(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("GetAllPostByLikedUsersTest/{id}/{start}/{end}")]
    public ActionResult GetAllPostByLikedUsers([FromRoute] int id, int start, int end)
    {
        try
        {
            return Ok(_service.GetAllPostByLikedUsers(id, start,end));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}