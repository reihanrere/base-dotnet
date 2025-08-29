namespace BaseDotnet.Validation;
    
using BaseDotnet.Entities;
using FluentValidation;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("The 'Email' not valid format");

        RuleFor(x => x.Password)
            .MinimumLength(3)
            .WithMessage("The 'Password' should have at least 3 characters.")
            .When(x => x.UserID == 0);

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