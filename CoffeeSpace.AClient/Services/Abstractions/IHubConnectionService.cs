using System;
using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.Models;

namespace CoffeeSpace.AClient.Services.Abstractions;

public interface IHubConnectionService
{
    public bool IsConnected { get; }

    Task StartConnectionAsync(string buyerId, CancellationToken cancellationToken);

    void OnOrderCreated(Action<Order> onOrderCreation);

    void OnOrderStatusUpdated(Action<OrderStatus, string> onOrderStatusUpdated);

    void OnOrderPaymentPageInitialized(Action<string, string> onOrderPageInitialized);
}
