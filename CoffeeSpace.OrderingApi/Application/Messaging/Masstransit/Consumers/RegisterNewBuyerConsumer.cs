using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Messages.Buyers.Commands;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;

internal sealed class RegisterNewBuyerConsumer : IConsumer<RegisterNewBuyer>
{
    private readonly IBuyerService _buyerService;
    
    public RegisterNewBuyerConsumer(IBuyerService buyerService)
    {
        _buyerService = buyerService;
    }

    public Task Consume(ConsumeContext<RegisterNewBuyer> context)
    {
        return _buyerService.CreateAsync(new Buyer
        {
            Name = context.Message.Name,
            Email = context.Message.Email
        }, context.CancellationToken);
    }
}