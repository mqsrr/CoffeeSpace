using System.Collections.ObjectModel;
using CoffeeSpace.Client.WebApiClients;
using CoffeeSpace.Domain.Ordering.Orders;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class CartViewModel : ObservableObject
{
    private readonly ISender _sender;
    private readonly IBuyersWebApi _buyersWebApi;

    [ObservableProperty]
    private ObservableCollection<OrderItem> _orderItems;
    
    public CartViewModel(ISender sender, IBuyersWebApi buyersWebApi)
    {
        _sender = sender;
        _buyersWebApi = buyersWebApi;
    }

    [RelayCommand]
    private void ClearCart()
    {
        OrderItems.AsParallel().ForAll(x => x.Quantity = 0);
        OrderItems.Clear();
    }

    [RelayCommand]
    internal void AddOrderItem(OrderItem orderItem)
    {
        orderItem.Quantity++;
        if (!OrderItems.Contains(orderItem))
        {
            OrderItems.Add(orderItem);
            return;
        }

        RefreshOrderItem(orderItem);
    }

     [RelayCommand]
     private async Task CreateOrder(CancellationToken cancellationToken)
     {
         ClearCart();
     }

    private void RefreshOrderItem(OrderItem orderItem)
    {
        OrderItems.Remove(orderItem);
        OrderItems.Add(orderItem);
    }
}