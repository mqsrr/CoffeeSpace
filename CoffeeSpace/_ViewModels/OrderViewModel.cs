using CoffeeSpace.Data.Models.Orders;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace CoffeeSpace._ViewModels;

public partial class OrderViewModel : ObservableObject
{

	[ObservableProperty]
    private ObservableCollection<Order> _orders;

    public OrderViewModel()
    {
	    _orders = new ObservableCollection<Order>();
    }
}
