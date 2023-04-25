using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;
using FluentValidation;

namespace CoffeeSpace.OrderingApi.Application.Validators.Orders;

internal sealed class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderRequestValidator()
    {
        RuleFor(x => x.Id)
            .Empty();
            
        RuleFor(x => x.BuyerId)
            .Empty();
            
        RuleFor(x => x.Address)
            .NotEmpty();
        
        RuleFor(x => x.PaymentInfo)
            .Empty();
            
        RuleFor(x => x.Status)
            .Empty();
        
        RuleFor(x => x.OrderItems)
            .NotNull()
            .ForEach(x => x.NotNull());
    }
}