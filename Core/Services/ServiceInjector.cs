using BaseDotnet.Modules.Users.Services;
using BaseDotnet.Modules.Roles.Services;
using FluentValidation;
using BaseDotnet.Modules.Users.Validations;
using BaseDotnet.Core.Entities;
using BaseDotnet.Modules.Auth.Validations;
using BaseDotnet.Core.Models;
using BaseDotnet.Modules.Roles.Validations;
using BaseDotnet.Modules.Users.Models;

namespace BaseDotnet.Core.Services;

public static class ServiceInjector
{
    public static void Inject(IServiceCollection services)
    {
        // Inject HttpContextAccessor
        services.AddHttpContextAccessor();

        // Inject Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();

        // Inject Validators
        services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();
        services.AddScoped<IValidator<Role>, RoleValidator>();
        services.AddScoped<IValidator<SignUpRequest>, SignUpValidation>();
    }
}