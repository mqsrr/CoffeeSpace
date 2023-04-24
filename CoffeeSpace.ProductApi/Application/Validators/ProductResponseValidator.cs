using CoffeeSpace.ProductApi.Application.Contracts.Responses;
using FluentValidation;

namespace CoffeeSpace.ProductApi.Application.Validators;

internal sealed class ProductResponseValidator : AbstractValidator<ProductResponse>
{
    public ProductResponseValidator()
    {
        RuleFor(x => x.Id)
            .NotNull();
            
        RuleFor(x => x.Title)
            .NotNull();
            
        RuleFor(x => x.Description)
            .NotNull();
            
        RuleFor(x => x.Discount)
            .NotNull();
        
        RuleFor(x => x.UnitPrice)
            .NotNull();
        
        RuleFor(x => x.Total)
            .NotNull();
    }
}