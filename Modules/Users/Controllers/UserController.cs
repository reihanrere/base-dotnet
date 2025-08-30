using BaseDotnet.Core.Controllers;
using BaseDotnet.Core.Entities;
using BaseDotnet.Modules.Users.Models;
using BaseDotnet.Modules.Users.Services;
using BaseDotnet.Modules.Users.Validations;

namespace BaseDotnet.Modules.Users.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UserController : BaseController
{
    private IUserService userService;
    private readonly ILogger<UserController> _logger;
    private const string TAG = "User {0}";
    private const string LOG = "User {x}";
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        this.userService = userService;
        _logger = logger;
    }

    // [Authorize(Page.MasterUser,Action.Read)]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery(Name = "keyword")] string? keyword = null,
        [FromQuery(Name = "orderBy")] string? orderBy = "UserID",
        [FromQuery(Name = "orderType")] string? orderType = "asc",
        [FromQuery(Name = "role")] int? role = null,
        [FromQuery(Name = "page")] int page = 1,
        [FromQuery(Name = "status")] string? status = null,
        [FromQuery(Name = "pageSize")] int pageSize = 10)
    {
        try
        {
            var user = await userService.GetAll(keyword, status, role, page, pageSize, orderBy, orderType);
            if (user == null)
                return _NotFound(string.Format(TAG, "Not Found"));

            return Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError(LOG, e);
            return _BadRequest(e.Message);
        }

    }

    // [Authorize(Page.MasterUser,Action.Create)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest item)
    {
        try
        {
            if (item == null)
                return _BadRequest(string.Format(TAG, "Body Null"));

            var userValidator = new CreateUserValidator();
            var result = userValidator.Validate(item);

            if (result.IsValid)
            {
                var id = await userService.Add(item);
                return _Ok(id, string.Format(TAG, "data has been successfully saved"));
            }
            else
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return _BadRequest(errorMessages);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(LOG, e);
            return _BadRequest(e.Message);
        }
    }

    // [Authorize(Page.MasterUser,Action.Update)]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequest item)
    {
        try
        {
            if (item == null)
                return _BadRequest(string.Format(TAG, "Body Null"));

            var userValidator = new UpdateUserValidator();
            var result = userValidator.Validate(item);
            if (result.IsValid)
            {
                var id = await userService.Update(item);
                return _Ok(id, string.Format(TAG, "data has been successfully updated"));
            }
            else
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return _BadRequest(errorMessages);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(LOG, e);
            return _BadRequest(e.Message);
        }
    }

    // [Authorize(Page.MasterUser,Action.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var data = await userService.GetByID(id);
            if (data == null)
                return _NotFound(string.Format(TAG, "Not Found"));

            await userService.Remove(data);

            return _Ok(id, string.Format(TAG, "data has been successfully deleted"));
        }
        catch (Exception e)
        {
            _logger.LogError(LOG, e);
            return _BadRequest(e.Message);
        }
    }

    // [Authorize(Page.MasterUser,Action.Read)]
    [HttpGet("{id}")]
    public async Task<IActionResult> getDetail(int id)
    {
        try
        {
            var data = await userService.GetByID(id);
            if (data == null)
                return _NotFound(string.Format(TAG, "Not Found"));

            data.Password = "";
            data.Salt = "";
            return _Ok(data);
        }
        catch (Exception e)
        {
            _logger.LogError(LOG, e);
            return _BadRequest(e.Message);
        }
    }
}


