﻿using CoffeeSpace._ViewModels;
using CoffeeSpace.Data.Models.Orders;
using CoffeeSpace.Messages.Requests;
using CoffeeSpace.Services.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;

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
        if (_hubConnection.State != HubConnectionState.Connected) 
            await _hubConnection.StartAsync(cancellationToken);
        
        Order order = await _orderRepository.CreateAsync(request.OrderItems, request.Customer, cancellationToken);
        _orderViewModel.Orders.Add(order);
        
        await _hubConnection.SendAsync("SendOrder", order, cancellationToken: cancellationToken);
        
        return Unit.Value;
    }

    public void Dispose() => _hubConnection.StopAsync();
}