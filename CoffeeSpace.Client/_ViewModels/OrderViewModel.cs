using CoffeeSpace.Client.Models.Ordering;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class OrderViewModel : ObservableObject
{
    [ObservableProperty] 
    private ICollection<Order> _orders;
    
    [RelayCommand]
    private void CancelOrder(Order order)
    {
        Orders.Remove(order);
    }
}