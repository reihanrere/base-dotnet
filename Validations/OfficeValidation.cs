namespace BaseDotnet.Validation;

using BaseDotnet.Entities;
using BaseDotnet.Models;
using FluentValidation;

public class OfficeValidator : AbstractValidator<Office>
{
    public OfficeValidator()
    {
        RuleFor(x => x.OfficeName)
            .NotNull()
            .NotEmpty()
            .WithMessage("The 'OfficeName' is required");
        RuleFor(x => x.OfficeAddress)
            .NotNull()
            .NotEmpty()
            .WithMessage("The 'OfficeAddress' is required");
        RuleFor(x => x.OfficeAddress)
            .NotNull()
            .NotEmpty()
            .WithMessage("The 'OfficeAddress' is required");
        RuleFor(x => x.OfficeLatitude)
            .NotNull()
            .NotEmpty()
            .WithMessage("The 'OfficeLatitude' is required");
        RuleFor(x => x.OfficeLongitude)
            .NotNull()
            .NotEmpty()
            .WithMessage("The 'OfficeLongitude' is required");
    }
}