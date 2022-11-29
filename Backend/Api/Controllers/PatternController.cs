using Application;
using Application.DTOs;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PatternController : ControllerBase
{

    private IPatternService _service;
    public PatternController(IPatternService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("GetAllPatterns")]
    public ActionResult GetAllPatterns()
    {
        try
        {
            return Ok(_service.GetAllPattern());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
    [HttpPost]
    [Route("CreatePattern")]
    public ActionResult CreatePattern( [FromBody] PatternDTO dto)
    {
        try
        {
            return Ok(_service.CreatePattern(dto));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut]
    [Route("UpdatePattern")]
    public ActionResult UpdatePattern( [FromBody] PatternUpdateDTO pattern)
    {
        try
        {
            return Ok(_service.UpdatePattern(pattern));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
    [HttpDelete]
    [Route("DeletePattern/{id}")]
    public ActionResult DeletePattern( [FromRoute] int id)
    {
        try
        {
            return Ok(_service.DeletePattern(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("GetPatternById/{id}")]
    public ActionResult GetPatternById( [FromRoute] int id)
    {
        try
        {
            return Ok(_service.GetPatternById(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpGet]
    [Route("GetAllPatternsByUser/{id}")]
    public ActionResult GetAllPatternsByUser( [FromRoute] int id)
    {
        try
        {
            return Ok(_service.GetAllPatternsByUser(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}