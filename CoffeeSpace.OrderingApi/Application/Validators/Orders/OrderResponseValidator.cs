using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Orders;
using FluentValidation;

namespace CoffeeSpace.OrderingApi.Application.Validators.Orders;

internal sealed class OrderResponseValidator : AbstractValidator<OrderResponse>
{
    public OrderResponseValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Address)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.Status)
            .NotEmpty();
            
        RuleFor(x => x.OrderItems)
            .NotEmpty()
            .NotNull();

        RuleForEach(x => x.OrderItems)
            .NotNull()
            .SetValidator(new OrderItemValidator());
    }
}