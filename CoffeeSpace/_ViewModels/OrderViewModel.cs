using CoffeeSpace.Data.Models.Orders;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeSpace._ViewModels;

public partial class OrderViewModel : ObservableObject
{

	[ObservableProperty] 
	private ObservableCollection<Order> _orders;


	public OrderViewModel()
	{
		
		_orders = new ObservableCollection<Order>();
	}

	[RelayCommand]
	private void CancelOrder(Order order)
		=> Orders.Remove(order);
}
