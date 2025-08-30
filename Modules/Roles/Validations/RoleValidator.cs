using BaseDotnet.Core.Entities;
using FluentValidation;

namespace BaseDotnet.Modules.Roles.Validations;

public class RoleValidator : AbstractValidator<Role>
{
    public RoleValidator()
    {
        RuleFor(x => x.RoleName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Role name is required")
            .Length(2, 100)
            .WithMessage("Role name must be between 2 and 100 characters")
            .Matches("^[a-zA-Z0-9\\s]+$")
            .WithMessage("Role name can only contain letters, numbers, and spaces");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.Permissions)
            .Must(BeValidJsonOrNull)
            .WithMessage("Permissions must be valid JSON format or null");

        // Prevent modification of system roles
        When(x => x.RoleID > 0, () => {
            RuleFor(x => x.RoleID)
                .Must(id => id > 4)
                .WithMessage("Cannot modify system default roles")
                .When(x => x.RoleID <= 4);
        });
    }

    private bool BeValidJsonOrNull(string? permissions)
    {
        if (string.IsNullOrEmpty(permissions))
            return true;

        try
        {
            System.Text.Json.JsonDocument.Parse(permissions);
            return true;
        }
        catch
        {
            return false;
        }
    }
}