using Microsoft.AspNetCore.Mvc;

namespace BaseDotnet.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : BaseController
{
    [HttpGet("/")]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }
}