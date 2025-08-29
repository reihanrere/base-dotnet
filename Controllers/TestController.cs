namespace BaseDotnet.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TestController : BaseController
{
    private readonly ILogger<TestController> _logger;
    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<IActionResult> Test()
    {
        return _Ok("api successfully get");
    }

}


