using CoffeeSpace.ProductApi.Application.Contracts.Responses;
using FluentValidation;

namespace CoffeeSpace.ProductApi.Application.Validators;

internal sealed class ProductResponseValidator : AbstractValidator<ProductResponse>
{
    public ProductResponseValidator()
    {
        RuleFor(response => response.Id)
            .NotEmpty()
            .NotNull();
            
        RuleFor(response => response.Title)
            .NotEmpty()
            .NotNull();

        RuleFor(response => response.Image)
            .NotNull();
            
        RuleFor(response => response.Description)
            .NotEmpty()
            .NotNull();
        
        RuleFor(response => response.UnitPrice)
            .NotNull()
            .GreaterThan(0)
            .LessThan(99);
        
        RuleFor(response => response.Quantity)
            .NotNull()
            .GreaterThan(0);
    }
}