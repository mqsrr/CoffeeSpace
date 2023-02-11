using CoffeeSpace.Data.Models.Orders;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.Messages.Requests;
using CoffeeSpace.Services.Repository;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace CoffeeSpace._ViewModels;

public partial class CartViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<OrderItem> _orderItems;
    
    private readonly IMediator _mediator;
    private readonly IRepository<Customer> _customerRepository;

    public CartViewModel(IMediator mediator, IRepository<Customer> customerRepository)
    {
        _orderItems = new ObservableCollection<OrderItem>();
        
        _mediator = mediator;
        _customerRepository = customerRepository;
    }

    [RelayCommand]
    private void ClearCart() => OrderItems.Clear();

    [RelayCommand]
    internal void AddOrderItem(OrderItem orderItem)
    {
        if (!OrderItems.Contains(orderItem))
        {
            OrderItems.Add(orderItem);
            return;
        }

        orderItem.Quantity++;
        RefreshOrderItem(orderItem);
    }

    [RelayCommand]
    private async Task CreateOrder()
    {
        await _mediator.Send(new CreateOrderRequest(OrderItems, await _customerRepository.GetByIdAsync("638BC3AA-7CC5-49A6-BBE1-6842EDF22F78")));
        ClearCart();
    }

    private void RefreshOrderItem(OrderItem orderItem)
    {
        OrderItems.Remove(orderItem);
        OrderItems.Add(orderItem);
    }
}
