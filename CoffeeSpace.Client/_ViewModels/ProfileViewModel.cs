using CoffeeSpace.Client.Models.Ordering;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty] 
    private Buyer _buyer;

    public bool IsPasswordVisible { get; set; }

    [RelayCommand]
    private void ChangePasswordVisibility() => IsPasswordVisible = !IsPasswordVisible;

    [RelayCommand]
    private Task ChangePassword()
    {
        return Task.CompletedTask;
    }
}