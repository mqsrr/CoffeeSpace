using CoffeeSpace.Client.Messages.Commands;
using CoffeeSpace.Client.Models.Ordering;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class CartViewModel : ObservableObject
{
    private readonly ISender _sender;

    [ObservableProperty]
    private ObservableCollection<OrderItem> _orderItems;

    public CartViewModel(ISender sender)
    {
        _sender = sender;
        _orderItems = new ObservableCollection<OrderItem>();
    }

    [RelayCommand]
    internal void AddOrderItemIntoCart(OrderItem orderItemToAdd)
    {
        var orderItem = OrderItems.FirstOrDefault(orderItem => orderItem.Title.Equals(orderItemToAdd.Title, StringComparison.OrdinalIgnoreCase));
        if (OrderItems.IsNullOrEmpty() || orderItem is null)
        {
            OrderItems.Add(orderItemToAdd);
            return;
        }
        orderItem.Quantity += orderItemToAdd.Quantity;

        OrderItems.Remove(orderItem);
        OrderItems.Add(orderItem);
    }

    [RelayCommand]
    private async Task ConfirmOrdersStock(CancellationToken cancellationToken)
    {
        await _sender.Send(new ConfirmOrdersStockCommand
        {
            OrderItems = OrderItems
        }, cancellationToken);

        OrderItems.Clear();
        await Shell.Current.GoToAsync("Order Information");
    }
}