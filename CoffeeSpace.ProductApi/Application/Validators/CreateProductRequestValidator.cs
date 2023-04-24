using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using FluentValidation;

namespace CoffeeSpace.ProductApi.Application.Validators;

internal sealed class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotNull();
            
        RuleFor(x => x.Description)
            .NotNull();
        
        RuleFor(x => x.Discount)
            .NotNull();
        
        RuleFor(x => x.UnitPrice)
            .NotNull();
            
        RuleFor(x => x.Quantity)
            .NotNull();

    }
}