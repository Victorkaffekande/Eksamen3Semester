using Application;
using Application.DTOs;
using Application.Interfaces;
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
}