using BaseDotnet.Core.Controllers;
using BaseDotnet.Core.Models;
using BaseDotnet.Modules.Auth.Validations;
using BaseDotnet.Modules.Role.Services;
using BaseDotnet.Modules.User.Services;

namespace BaseDotnet.Modules.Auth.Controllers;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("[controller]")]
public class AuthController : BaseController
{
    private IUserService userService;
    private readonly ILogger<AuthController> _logger;
    private const string TAG = "Auth {error}";
    private const string LOG = "User {x}";
    IRoleService roleService;
    public AuthController(IUserService userService, IRoleService roleService, ILogger<AuthController> logger)
    {
        this.userService = userService;
        this.roleService = roleService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest model)
    {
        try
        {
            var response = userService.Authenticate(model);
            if (response == null)
            {
                _logger.LogError(TAG, "Email or password is incorrect");
                return _BadRequest("Email or password is incorrect");
            }

            var user = await userService.GetByID(response.UserID);
            var role = await roleService.GetByID(user.RoleID);
            if (role != null)
            {
                Console.WriteLine(role);
                role.AccessList = await roleService.GetPageAccessByRoleID(role.RoleID);
                _logger.LogInformation("LOGIN USER " + JsonConvert.SerializeObject(role));
                response.role = role;
            }

            return _Ok(response);

        }
        catch (Exception e)
        {
            _logger.LogError(TAG, e);
            return _BadRequest(e);
        }

    }

    [HttpPost("signup")]
    public async Task<IActionResult> Create([FromBody] SignUpRequest item)
    {
        try
        {
            if (item == null)
                return _BadRequest(string.Format(TAG, "Body Null"));

            var userValidator = new SignUpValidation();
            var result = userValidator.Validate(item);

            if (result.IsValid)
            {
                var id = await userService.SignUp(item);
                return _Ok(id, string.Format(TAG, "data has been successfully created"));
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
}


