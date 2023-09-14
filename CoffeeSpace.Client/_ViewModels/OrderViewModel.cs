using System.Collections.ObjectModel;
using CoffeeSpace.Client.Models.Ordering;
using CoffeeSpace.Client.Views;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class OrderViewModel : ObservableObject
{
    [ObservableProperty] 
    private ObservableCollection<Order> _orders;

    public OrderViewModel(ProfileViewModel profileViewModel)
    {
        _orders = profileViewModel.Buyer.Orders.ToObservableCollection() ?? new ObservableCollection<Order>();
    }
}