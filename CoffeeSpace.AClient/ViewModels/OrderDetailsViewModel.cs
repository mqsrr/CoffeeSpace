using CoffeeSpace.AClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeSpace.AClient.ViewModels;

public sealed partial class OrderDetailsViewModel : ViewModelBase
{
    [ObservableProperty]
    private Order _order = null!;

    public OrderDetailsViewModel()
    {
        
    }
    
    public OrderDetailsViewModel(Order order)
    {
        Order = order;
    }
}