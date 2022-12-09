﻿using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }
    
    [HttpGet]
    [Route("GetUserById/{id}")]
    public ActionResult GetUserById( [FromRoute] int id)
    {

        try
        {
            return Ok(_service.GetUserById(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("GetAllUsers")]
    public ActionResult GetAllUser()
    {

        try
        {
            return Ok(_service.GetAllUsers());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut]
    [Route("UpdateUser")]
    public ActionResult UpdatePattern( [FromBody] UserDTO pattern)
    {
        try
        {
            return Ok(_service.UpdateUser(pattern));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}