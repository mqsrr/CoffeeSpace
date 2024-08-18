using CoffeeSpace.Domain.Ordering.Orders;
using FluentValidation;

namespace CoffeeSpace.OrderingApi.Application.Validators.Orders;

internal sealed class OrderItemRequestValidator : AbstractValidator<OrderItem>
{
    public OrderItemRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull();
            
        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .MaximumLength(200);
        
        RuleFor(x => x.Discount)
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