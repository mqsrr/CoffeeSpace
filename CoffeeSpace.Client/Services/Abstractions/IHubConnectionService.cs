using CoffeeSpace.Client.Models.Ordering;

namespace CoffeeSpace.Client.Services.Abstractions;

public interface IHubConnectionService
{
    public bool IsConnected { get; }

    Task StartConnectionAsync(string buyerId, CancellationToken cancellationToken);

    void OrderCreated(Action<Order> onOrderCreation);

    void OrderStatusUpdated(Action<OrderStatus, string> onOrderStatusUpdated);
}
