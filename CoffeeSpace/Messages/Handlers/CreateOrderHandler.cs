using System.Collections.ObjectModel;
using CoffeeSpace._ViewModels;
using CoffeeSpace.Data.Models.Orders;
using CoffeeSpace.Messages.Requests;
using CoffeeSpace.Services.Repository;
using CommunityToolkit.Maui.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using static System.GC;

namespace CoffeeSpace.Messages.Handlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderRequest>, IDisposable
{
    private readonly HubConnection _hubConnection;
    private readonly IOrderRepository _orderRepository;
    private readonly OrderViewModel _orderViewModel;

    public CreateOrderHandler(HubConnection hubConnection, IOrderRepository orderRepository, OrderViewModel orderViewModel)
    {
        _hubConnection = hubConnection;
        _orderRepository = orderRepository;
        _orderViewModel = orderViewModel;
    }

    public async Task<Unit> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        if (_hubConnection.State is not HubConnectionState.Connected)
            await _hubConnection.StartAsync(cancellationToken);

        Order order = await _orderRepository.CreateAsync(request.OrderItems, request.Customer, cancellationToken);

        if (order.OrderItems is not ObservableCollection<OrderItem>)
            order.OrderItems = order.OrderItems.ToObservableCollection();
        
        _orderViewModel.Orders.Add(order);

        await _hubConnection.SendAsync("SendOrder", order, cancellationToken: cancellationToken);
        
        return Unit.Value;
    }

    public void Dispose()
    {
        Task.Run(() => _hubConnection.StopAsync());
        SuppressFinalize(this);
    }
}