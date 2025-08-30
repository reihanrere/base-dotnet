using BaseDotnet.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BaseDotnet.Core.Helpers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[]? _allowedRoles;

    public AuthorizeAttribute(params string[] allowedRoles)
    {
        _allowedRoles = allowedRoles;
    }

    public AuthorizeAttribute()
    {
        _allowedRoles = null;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (User)context.HttpContext.Items["User"];
        
        if (user == null)
        {
            context.Result = new JsonResult(new { message = "Unauthorized - Please login" }) 
                { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }

        if (_allowedRoles != null && _allowedRoles.Length > 0)
        {
            var hasAccess = _allowedRoles.Any(role => 
                string.Equals(role, user.RoleName, StringComparison.OrdinalIgnoreCase));

            if (!hasAccess)
            {
                context.Result = new JsonResult(new { message = "Forbidden - Insufficient permissions" }) 
                    { StatusCode = StatusCodes.Status403Forbidden };
                return;
            }
        }
    }
}