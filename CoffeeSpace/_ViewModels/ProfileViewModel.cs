using CoffeeSpace.Data.Models.CustomerInfo;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeSpace._ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty]
    private Customer _customer;

    public bool IsPasswordVisible { get; set; } = false;

    [RelayCommand]
    public void ChangePasswordVisibility() => IsPasswordVisible = !IsPasswordVisible;

    [RelayCommand]
    public async Task ChangePassword(CancellationToken cancellationToken)
    {
        
    }
}