using BaseDotnet.Core.Models;

namespace BaseDotnet.Modules.Auth.Validations;

using FluentValidation;

public class SignUpValidation : AbstractValidator<SignUpRequest>
{
    public SignUpValidation()
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

        RuleFor(x => x.DisplayName)
            .NotNull()
            .NotEmpty()
            .WithMessage("The 'Display Name' is required");
    }
}