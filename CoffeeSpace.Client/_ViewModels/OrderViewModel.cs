using System.Collections.ObjectModel;
using CoffeeSpace.Domain.Ordering.Orders;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class OrderViewModel : ObservableObject
{
    [ObservableProperty] 
    private ObservableCollection<Order> _orders;

    public OrderViewModel()
    {
        _orders = new ObservableCollection<Order>();
    }

    [RelayCommand]
    private void CancelOrder(Order order)
    {
        Orders.Remove(order);
    }
}