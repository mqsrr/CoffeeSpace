using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using FluentValidation;

namespace CoffeeSpace.ProductApi.Application.Validators;

internal sealed class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
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