using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
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
}