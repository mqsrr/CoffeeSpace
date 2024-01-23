using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoBogus;
using CoffeeSpace.AClient.Models;
using CoffeeSpace.AClient.Services;
using CoffeeSpace.AClient.Services.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace CoffeeSpace.AClient.ViewModels;

public sealed partial class OrderHistoryWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Order> _orders;

    private readonly IHubConnectionService _hubConnection;
    private readonly ILogger<OrderHistoryWindowViewModel> _logger;

    public OrderHistoryWindowViewModel(IHubConnectionService hubConnection, ILogger<OrderHistoryWindowViewModel> logger)
    {
        _hubConnection = hubConnection;
        _logger = logger;
        Orders = new ObservableCollection<Order>(AutoFaker.Generate<Order>(10));
    }

    public OrderHistoryWindowViewModel()
    {
        _logger = null!;
        _hubConnection = null!;
        Orders = new ObservableCollection<Order>(AutoFaker.Generate<Order>(10));
    }

    internal async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? buyerId = StaticStorage.BuyerId;
        if (buyerId is null)
        {
            return;
        }

        if (_hubConnection.IsConnected)
        {
            return;
        }
        
        await _hubConnection.StartConnectionAsync(buyerId, cancellationToken);
        
        _hubConnection.OnOrderCreated(order => Orders.Add(order));
        _hubConnection.OnOrderStatusUpdated((status, orderId) =>
        {
            var orderToUpdate = Orders.FirstOrDefault(order => order.Id == orderId);
            if (orderToUpdate is null)
            {
                return;
            }

            orderToUpdate.Status = status;
        });
        
        _hubConnection.OnOrderPaymentPageInitialized((orderId, paymentLink) =>
        {
            _logger.LogInformation("Payment Link has been initialized for {OrderId}: {PaymentLink}", orderId, paymentLink);                        
        });
    }
}