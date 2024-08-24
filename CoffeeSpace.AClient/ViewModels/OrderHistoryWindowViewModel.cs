using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CoffeeSpace.AClient.Models;
using CoffeeSpace.AClient.Services;
using CoffeeSpace.AClient.Services.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeSpace.AClient.ViewModels;

public sealed partial class OrderHistoryWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Order> _orders;

    private readonly IHubConnectionService _hubConnectionService;

    public OrderHistoryWindowViewModel(IHubConnectionService hubConnectionService)
    {
        _hubConnectionService = hubConnectionService;
        Orders = new ObservableCollection<Order>(StaticStorage.Buyer!.Orders ?? ArraySegment<Order>.Empty);
    }

    public OrderHistoryWindowViewModel()
    {
        _hubConnectionService = null!;
        Orders = new ObservableCollection<Order>();
    }

    internal async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (!_hubConnectionService.IsConnected)
        {
            await _hubConnectionService.StartConnectionAsync(StaticStorage.Buyer!.Id, cancellationToken);
        }
        
        _hubConnectionService.OnOrderCreated(order => Orders.Add(order));
        _hubConnectionService.OnOrderStatusUpdated((status, orderId) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                var orderToUpdate = Orders.FirstOrDefault(order => order.Id == orderId);
                if (orderToUpdate is null)
                {
                    return;
                }

                orderToUpdate.Status = status;
            });
        }); 
    }
}