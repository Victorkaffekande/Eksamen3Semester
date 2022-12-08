using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private IPostService _service;
    public PostController(IPostService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("CreatePost")]
    public ActionResult CreatePost( [FromBody] PostCreateDTO dto)
    {
        try
        {
            return Ok(_service.CreatePost(dto));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
      
    [HttpPost]
    [Route("UpdatePost")]
    public ActionResult UpdatePost( [FromBody] PostUpdateDTO dto)
    {
        try
        {
            return Ok(_service.UpdatePost(dto));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
      
    [HttpDelete]
    [Route("DeletePost/{id}")]
    public ActionResult DeletePost( [FromRoute] int id)
    {
        try
        {
            return Ok(_service.DeletePost(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("GetPostById/{id}")]
    public ActionResult GetPostById( [FromRoute] int id)
    {
        try
        {
            return Ok(_service.GetPostById(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpGet]
    [Route("GetAllPostByProject/{id}")]
    public ActionResult GetAllPostByProject( [FromRoute] int id)
    {
        try
        {
            return Ok(_service.GetAllPostFromProject(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
}