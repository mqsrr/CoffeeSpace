using System.Collections.ObjectModel;
using CoffeeSpace._ViewModels;
using CoffeeSpace.Data.Models.Orders;
using CoffeeSpace.Messages.Requests;
using CoffeeSpace.Services;
using CommunityToolkit.Maui.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using static System.GC;

namespace CoffeeSpace.Messages.Handlers;

public class CreateOrderRequestHandler : IRequestHandler<CreateOrderRequest>, IDisposable
{
    private readonly HubConnection _hubConnection;
    private readonly OrderViewModel _orderViewModel;
    private readonly IServiceDataProvider<Order> _orderServiceData;

    public CreateOrderRequestHandler(HubConnection hubConnection, OrderViewModel orderViewModel, IServiceDataProvider<Order> orderServiceData)
    {
        _hubConnection = hubConnection;
        _orderViewModel = orderViewModel;
        _orderServiceData = orderServiceData;
    }

    public async Task<Unit> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        if (_hubConnection.State is not HubConnectionState.Connected)
            await _hubConnection.StartAsync(cancellationToken);

        Order order = new Order
        {
            OrderItems = request.OrderItems,
            CustomerId = request.Customer.Id,
            Customer = request.Customer,
            AdmittedTime = DateTime.Now,
            Status = OrderStatus.Submitted
        };

        // await _orderService.AddAsync(order, cancellationToken);
        
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