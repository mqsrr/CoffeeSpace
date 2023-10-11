using CoffeeSpace.Client.Models.Ordering;

namespace CoffeeSpace.Client.Services.Abstractions;

public interface IHubConnectionService
{
    public bool IsConnected { get; }

    Task StartConnectionAsync(string buyerId, CancellationToken cancellationToken);

    void OnOrderCreated(Action<Order> onOrderCreation);

    void OnOrderStatusUpdated(Action<OrderStatus, string> onOrderStatusUpdated);

    void OnOrderPaymentPageInitialized(Action<string> onOrderPageInitialized);
}
