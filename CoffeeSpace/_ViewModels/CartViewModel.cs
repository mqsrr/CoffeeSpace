using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CoffeeSpace.Application.Models.CustomerInfo;
using CoffeeSpace.Application.Models.Orders;
using CoffeeSpace.Messages.Requests;
using CoffeeSpace.Services;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace CoffeeSpace._ViewModels;

public partial class CartViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<OrderItem> _orderItems;

    private readonly IServiceDataProvider<Customer> _customerServiceData;
    private readonly ISender _sender;

    public CartViewModel(ISender sender, IServiceDataProvider<Customer> customerServiceData)
    {
        _orderItems = new ObservableCollection<OrderItem>();
        
        _sender = sender;
        _customerServiceData = customerServiceData;
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
        Customer currentCustomer =
            await _customerServiceData.GetByIdAsync(Guid.Parse("69a92976-98d9-456e-91fa-9114d0330873"), cancellationToken);
        
        await _sender.Send(new CreateOrderRequest(OrderItems, currentCustomer), cancellationToken);
        
        ClearCart();
    }

    private void RefreshOrderItem(OrderItem orderItem)
    {
        OrderItems.Remove(orderItem);
        OrderItems.Add(orderItem);
    }
}
