using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;
using FluentValidation;

namespace CoffeeSpace.OrderingApi.Application.Validators.Orders;

internal sealed class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.BuyerId)
            .Empty();
        
        RuleFor(x => x.Address)
            .NotNull()
            .NotEmpty();
            
        RuleFor(x => x.PaymentInfo)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.Status)
            .IsInEnum();
        
        RuleFor(x => x.OrderItems)
            .NotEmpty()
            .NotNull();

        RuleForEach(x => x.OrderItems)
            .NotNull()
            .SetValidator(new OrderItemValidator());
    }
}