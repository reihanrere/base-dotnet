namespace BaseDotnet.Modules.Auth.Controllers;

using BaseDotnet.Core.Controllers;
using BaseDotnet.Core.Models;
using BaseDotnet.Modules.Auth.Validations;
using BaseDotnet.Modules.Roles.Services;
using BaseDotnet.Modules.Users.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController : BaseController
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserService userService, IRoleService roleService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _roleService = roleService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest model)
    {
        try
        {
            var response = _userService.Authenticate(model);
            if (response == null)
            {
                return _BadRequest("Email or password is incorrect");
            }

            var user = await _userService.GetByID(response.UserID);
            var role = await _roleService.GetByID(user.RoleID);
            if (role != null)
            {
                response.role = role;
            }

            return _Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError("Auth error: {Error}", e);
            return _BadRequest(e.Message);
        }
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Create([FromBody] SignUpRequest item)
    {
        try
        {
            if (item == null)
                return _BadRequest("Request body is null");

            var userValidator = new SignUpValidation();
            var result = userValidator.Validate(item);

            if (result.IsValid)
            {
                var id = await _userService.SignUp(item);
                return _Ok(id, "User has been successfully created");
            }
            else
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return _BadRequest(errorMessages);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("SignUp error: {Error}", e);
            return _BadRequest(e.Message);
        }
    }
}