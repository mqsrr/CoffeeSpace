﻿using System.Buffers;
using CoffeeSpace.Data.Models.Orders;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CoffeeSpace.Data.Models.CustomerInfo;
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
    private readonly IMediator _mediator;

    public CartViewModel(IMediator mediator, IServiceDataProvider<Customer> customerServiceData)
    {
        _orderItems = new ObservableCollection<OrderItem>();
        
        _mediator = mediator;
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
    private async Task CreateOrder()
    {
        Customer currentCustomer =
            await _customerServiceData.GetByIdAsync(Guid.Parse("638BC3AA-7CC5-49A6-BBE1-6842EDF22F78"));
        
        await _mediator.Send(new CreateOrderRequest(OrderItems, currentCustomer));
        
        ClearCart();
    }

    private void RefreshOrderItem(OrderItem orderItem)
    {
        OrderItems.Remove(orderItem);
        OrderItems.Add(orderItem);
    }
}
