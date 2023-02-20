using CoffeeSpace.Data.Models.CustomerInfo;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeSpace._ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty]
    private Customer _customer;
}