using BaseDotnet.Modules.Users.Models;

namespace BaseDotnet.Modules.Users.Validations;

using BaseDotnet.Core.Entities;
using FluentValidation;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("The 'Email' not valid format");

        RuleFor(x => x.Password)
            .MinimumLength(3)
            .WithMessage("The 'Password' should have at least 3 characters.");

        RuleFor(x => x.UserName)
            .NotNull()
            .NotEmpty()
            .WithMessage("The 'UserName' is required");

        RuleFor(x => x.Status)
            .NotNull()
            .NotEmpty()
            .WithMessage("The 'Status' is required");

        RuleFor(x => x.DisplayName)
            .NotNull()
            .NotEmpty()
            .WithMessage("The 'Display Name' is required");

         RuleFor(x => x.Status)
            .Must(status => status == "Active" || status == "InActive")
            .WithMessage("Status must be 'Active' or 'InActive'");    
    }
    
    
}