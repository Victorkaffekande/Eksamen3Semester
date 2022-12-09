using Application;
using Application.DTOs;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ProjectController : Controller
{
    private IProjectService _service;

    public ProjectController(IProjectService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("GetById/{id}")]
    public ActionResult GetById([FromRoute] int id)
    {
        try
        {
            return Ok(_service.GetProjectById(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("GetProjectsFromUser/{id}")]
    public ActionResult GetAllProjectsFromUser([FromRoute] int id)
    {
        try
        {
            return Ok(_service.GetAllProjectsFromUser(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("GetProjects")]
    public ActionResult GetAllProjects()
    {
        try
        {
            return Ok(_service.GetAllProjects());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("CreateProject")]
    public ActionResult CreateProject([FromBody] ProjectCreateDTO createDto)
    {
        try
        {
            return Ok(_service.CreateProject(createDto));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Route("DeleteProject/{id}")]
    public ActionResult DeleteProject([FromRoute] int id)
    {
        try
        {
            return Ok(_service.DeleteProject(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Route("UpdateProject")]
    public ActionResult UpdateProject([FromBody] Project project)
    {
        try
        {
            return Ok(_service.UpdateProject(project));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}