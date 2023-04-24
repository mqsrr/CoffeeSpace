using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using FluentValidation;

namespace CoffeeSpace.ProductApi.Application.Validators;

internal sealed class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Id)
            .Null();

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