using CoffeeSpace.Domain.Ordering.Orders;
using FluentValidation;

namespace CoffeeSpace.OrderingApi.Application.Validators.Orders;

internal sealed class OrderItemValidator : AbstractValidator<OrderItem>
{
    public OrderItemValidator()
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