using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Messages.Buyers;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;

public sealed class RegisterNewBuyerConsumer : IConsumer<RegisterNewBuyer>
{
    private readonly IBuyerService _buyerService;
    private readonly ILogger<RegisterNewBuyerConsumer> _logger;
    
    public RegisterNewBuyerConsumer(IBuyerService buyerService, ILogger<RegisterNewBuyerConsumer> logger)
    {
        _buyerService = buyerService;
        _logger = logger;
    }

    public Task Consume(ConsumeContext<RegisterNewBuyer> context)
    {
        return _buyerService.CreateAsync(new Buyer
        {
            Id = Guid.NewGuid(),
            Name = context.Message.Name,
            Email = context.Message.Email
        }, context.CancellationToken);
    }
}