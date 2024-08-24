using System;
using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.Models;
using CoffeeSpace.AClient.Services.Abstractions;
using Microsoft.AspNetCore.SignalR.Client;

namespace CoffeeSpace.AClient.Services;

internal sealed class HubConnectionService : IHubConnectionService, IAsyncDisposable
{
    private readonly HubConnection _hubConnection;
    private string _buyerId = null!;

    public bool IsConnected => _hubConnection.State is HubConnectionState.Connected;

    public HubConnectionService()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"http://localhost:5245/ordering-hub")
            .WithAutomaticReconnect()
            .Build();
    }

    public async Task StartConnectionAsync(string buyerId, CancellationToken cancellationToken)
    {
        _buyerId = buyerId;

        await _hubConnection.StartAsync(cancellationToken);
        await _hubConnection.InvokeAsync("JoinGroup", buyerId, cancellationToken);
    }

    public void OnOrderCreated(Action<Order> onOrderCreation)
    {
        _hubConnection.On("OrderCreated", onOrderCreation);
    }

    public void OnOrderStatusUpdated(Action<OrderStatus, string> onOrderStatusUpdated)
    {
        _hubConnection.On("OrderStatusUpdated", onOrderStatusUpdated);
    }

    public void OnOrderPaymentPageInitialized(Action<string, string> onOrderPageInitialized)
    {
        _hubConnection.On("OrderPaymentPageInitialized", onOrderPageInitialized);
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.InvokeAsync("LeaveGroup", _buyerId);
        await _hubConnection.StopAsync();
    }
}
