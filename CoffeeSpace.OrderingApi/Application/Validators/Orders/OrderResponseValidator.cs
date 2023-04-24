using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Orders;
using FluentValidation;

namespace CoffeeSpace.OrderingApi.Application.Validators.Orders;

internal sealed class OrderResponseValidator : AbstractValidator<OrderResponse>
{
    public OrderResponseValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Address)
            .NotEmpty();
        
        RuleFor(x => x.Status)
            .NotEmpty();
            
        RuleFor(x => x.OrderItems)
            .NotNull()
            .ForEach(x => x.NotNull());
    }
}