using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.WebApiClients;
using Mediator;

namespace CoffeeSpace.Client.Messages.Commands.Handlers;

public sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, bool>
{
    private readonly IOrderingWebApi _orderingWebApi;

    public CreateOrderCommandHandler(IOrderingWebApi orderingWebApi)
    {
        _orderingWebApi = orderingWebApi;
    }

    public async ValueTask<bool> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var createdOrder = await _orderingWebApi.CreateOrder(command.BuyerId, command.CreateOrderRequest, cancellationToken);
        return createdOrder is not null;
    }
}
