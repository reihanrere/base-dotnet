namespace BaseDotnet.Validation;

using BaseDotnet.Entities;
using BaseDotnet.Models;
using FluentValidation;

public class ProductCategoryUpdateValidator : AbstractValidator<ProductCategory>
{
    public ProductCategoryUpdateValidator()
    {
        RuleFor(x => x.ProductCategoryName)
            .NotNull()
            .NotEmpty()
            .WithMessage("The 'ProductCategoryName' is required");
        RuleFor(x => x.ProductCategoryID)
            .GreaterThan(0)
            .WithMessage("The 'ProductCategoryID' is required and must be greater than 0");
    }
}