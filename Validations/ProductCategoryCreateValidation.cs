namespace BaseDotnet.Validation;

using BaseDotnet.Entities;
using BaseDotnet.Models;
using FluentValidation;

public class ProductCategoryCreateValidator : AbstractValidator<ProductCategoryRequest>
{
    public ProductCategoryCreateValidator()
    {
        RuleFor(x => x.ProductCategoryName)
            .NotNull()
            .NotEmpty()
            .WithMessage("The 'ProductCategoryName' is required");
    }
}