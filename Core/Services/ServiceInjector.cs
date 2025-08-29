using BaseDotnet.Modules.User.Services;
using BaseDotnet.Modules.Role.Services;
using FluentValidation;
using BaseDotnet.Modules.User.Validations;
using BaseDotnet.Core.Entities;
using BaseDotnet.Modules.Auth.Validations;
using BaseDotnet.Core.Models;

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
        services.AddScoped<IValidator<User>, UserValidator>();
        services.AddScoped<IValidator<SignUpRequest>, SignUpValidation>();
    }
}