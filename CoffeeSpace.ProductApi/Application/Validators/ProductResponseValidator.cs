using CoffeeSpace.ProductApi.Application.Contracts.Responses;
using FluentValidation;

namespace CoffeeSpace.ProductApi.Application.Validators;

internal sealed class ProductResponseValidator : AbstractValidator<ProductResponse>
{
    public ProductResponseValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull();
            
        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull();
            
        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull();
            
        RuleFor(x => x.Discount)
            .NotEmpty()
            .NotNull()
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(1);
        
        RuleFor(x => x.UnitPrice)
            .NotEmpty()
            .NotNull()
            .GreaterThan(0)
            .LessThan(99);
        
        RuleFor(x => x.Quantity)
            .NotEmpty()
            .NotNull()
            .GreaterThan(0);
    }
}