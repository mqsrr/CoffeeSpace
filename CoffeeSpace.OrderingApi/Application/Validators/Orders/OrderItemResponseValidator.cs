using CoffeeSpace.OrderingApi.Application.Contracts.Requests.OrderItems;
using FluentValidation;

namespace CoffeeSpace.OrderingApi.Application.Validators.Orders;

internal sealed class OrderItemResponseValidator : AbstractValidator<OrderItemResponse>
{
    public OrderItemResponseValidator()
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