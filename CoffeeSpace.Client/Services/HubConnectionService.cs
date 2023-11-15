using CoffeeSpace.Client.Models.Ordering;
using CoffeeSpace.Client.Services.Abstractions;
using Microsoft.AspNetCore.SignalR.Client;

namespace CoffeeSpace.Client.Services;

internal sealed class HubConnectionService : IHubConnectionService, IAsyncDisposable
{
    private readonly HubConnection _hubConnection;
    private string _buyerId;

    public bool IsConnected => _hubConnection.State is HubConnectionState.Connected;

    public HubConnectionService()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:8085/ordering-hub")
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
