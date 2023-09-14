using System.Net.Http.Json;
using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.Mappers;
using CoffeeSpace.Client.Models.Ordering;
using CoffeeSpace.Client.WebApiClients;
using Mediator;

namespace CoffeeSpace.Client.Messages.Commands.Handlers;

public sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, bool>
{
    private readonly OrderViewModel _orderViewModel;
    private readonly IOrderingWebApi _orderingWebApi;

    public CreateOrderCommandHandler(OrderViewModel orderViewModel, IOrderingWebApi orderingWebApi)
    {
        _orderViewModel = orderViewModel;
        _orderingWebApi = orderingWebApi;
    }

    public async ValueTask<bool> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var response = await _orderingWebApi.CreateOrder(command.BuyerId, command.CreateOrderRequest, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        
        var createdOrder = await response.Content.ReadFromJsonAsync<Order>(cancellationToken: cancellationToken);
        _orderViewModel.Orders.Add(createdOrder);
        return true;
    }
}
