using CoffeeSpace.Client.Models.Ordering;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class OrderViewModel : ObservableObject
{
    [ObservableProperty] 
    private ICollection<Order> _orders;

    public OrderViewModel()
    {
        _orders = new List<Order>();
    }
}