using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeSpace._ViewModels;

public sealed class ProfileViewModel
{
    [ObservableProperty]
    private readonly Customer _customer;
    
}