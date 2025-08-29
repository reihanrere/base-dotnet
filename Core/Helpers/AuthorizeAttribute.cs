
using BaseDotnet.Core.Entities;
using BaseDotnet.Modules.Role.Services;

namespace BaseDotnet.Core.Helpers;

using BaseDotnet.Core.Models.Enum;
using BaseDotnet.Modules.User.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter

{

    private readonly Page _page;
    private readonly Action _action;

    private IUserService _userService;

    public AuthorizeAttribute(Page page, Action action)
    {
        _page = page;
        _action = action;
    }
    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        var allow = true;
        var user = (User)context.HttpContext.Items["User"];
        if (user == null)
            allow = false;
        else
        {
            var roleService = context.HttpContext.RequestServices.GetService(typeof(IRoleService)) as IRoleService;
            var page = await roleService.GetPageAccessByPageCode(_page, user.RoleID);
            if (page == null)
                allow = false;
            else
            {
                string[] actionList = page.Action.Split(",");
                var hasAction = false;
                foreach (string action in actionList)
                {
                    if (action.Equals(_action.ToString()))
                    {
                        hasAction = true;
                        break;
                    }
                }

                if (!hasAction)
                    allow = false;
            }
        }

        if (!allow)
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };

    }


}