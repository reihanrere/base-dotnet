using BaseDotnet.Core.Controllers;
using BaseDotnet.Core.Entities;
using BaseDotnet.Core.Helpers;
using BaseDotnet.Modules.Roles.Services;
using BaseDotnet.Modules.Roles.Validations;
using Microsoft.AspNetCore.Mvc;

namespace BaseDotnet.Modules.Roles.Controllers;

[ApiController]
[Route("[controller]")]
public class RoleController : BaseController
{
    private readonly IRoleService _roleService;
    private readonly ILogger<RoleController> _logger;

    public RoleController(IRoleService roleService, ILogger<RoleController> logger)
    {
        _roleService = roleService;
        _logger = logger;
    }

    /// <summary>
    /// Get all roles with pagination and filtering
    /// </summary>
    [Authorize("Admin", "Manager")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery(Name = "keyword")] string? keyword = null,
        [FromQuery(Name = "orderBy")] string? orderBy = "RoleID",
        [FromQuery(Name = "orderType")] string? orderType = "asc",
        [FromQuery(Name = "page")] int page = 1,
        [FromQuery(Name = "pageSize")] int pageSize = 10)
    {
        try
        {
            var roles = await _roleService.GetAll(keyword, page, pageSize, orderBy, orderType);
            if (roles == null || !roles.Data.Any())
                return _NotFound("No roles found");

            return Ok(roles);
        }
        catch (Exception e)
        {
            _logger.LogError("Get all roles error: {Error}", e);
            return _BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get roles as options for dropdown/select
    /// </summary>
    [Authorize]
    [HttpGet("options")]
    public async Task<IActionResult> GetAllOptions()
    {
        try
        {
            var options = await _roleService.GetAllOption();
            return _Ok(options);
        }
        catch (Exception e)
        {
            _logger.LogError("Get role options error: {Error}", e);
            return _BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get role by ID
    /// </summary>
    [Authorize("Admin", "Manager")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetail(int id)
    {
        try
        {
            var role = await _roleService.GetByID(id);
            if (role == null)
                return _NotFound("Role not found");

            return _Ok(role);
        }
        catch (Exception e)
        {
            _logger.LogError("Get role detail error: {Error}", e);
            return _BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Create new role
    /// </summary>
    [Authorize("Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Role item)
    {
        try
        {
            if (item == null)
                return _BadRequest("Request body is null");

            var roleValidator = new RoleValidator();
            var result = roleValidator.Validate(item);

            if (result.IsValid)
            {
                var id = await _roleService.Add(item);
                return _Ok(id, "Role has been successfully created");
            }
            else
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return _BadRequest(errorMessages);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Create role error: {Error}", e);
            return _BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Update existing role
    /// </summary>
    [Authorize("Admin")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Role item)
    {
        try
        {
            if (item == null)
                return _BadRequest("Request body is null");

            var roleValidator = new RoleValidator();
            var result = roleValidator.Validate(item);

            if (result.IsValid)
            {
                var id = await _roleService.Update(item);
                return _Ok(id, "Role has been successfully updated");
            }
            else
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return _BadRequest(errorMessages);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Update role error: {Error}", e);
            return _BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Delete role by ID
    /// </summary>
    [Authorize("Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            // Check if role exists
            var role = await _roleService.GetByID(id);
            if (role == null)
                return _NotFound("Role not found");

            // Prevent deletion of default system roles
            if (role.RoleID <= 4) // Assuming first 4 roles are system defaults
                return _BadRequest("Cannot delete system default roles");

            await _roleService.Remove(role);
            return _Ok(id, "Role has been successfully deleted");
        }
        catch (Exception e)
        {
            _logger.LogError("Delete role error: {Error}", e);
            return _BadRequest(e.Message);
        }
    }
}