﻿using CoffeeSpace.Data.Models.Orders;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CoffeeSpace.Services.Repository;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeSpace._ViewModels;

public partial class OrderViewModel : ObservableObject
{

	[ObservableProperty] 
	private ObservableCollection<Order> _orders;

	private readonly IOrderRepository _orderRepository;

	public OrderViewModel(IOrderRepository orderRepository)
	{
		_orderRepository = orderRepository;
		
		_orders = new ObservableCollection<Order>();
	}

	[RelayCommand]
	private void CancelOrder(Order order)
		=> Orders.Remove(order);
}
