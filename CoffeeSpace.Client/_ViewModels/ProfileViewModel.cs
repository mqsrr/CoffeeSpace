using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty] 
    private Buyer _buyer;

    public bool IsPasswordVisible { get; set; }

    [RelayCommand]
    public void ChangePasswordVisibility() => IsPasswordVisible = !IsPasswordVisible;

    [RelayCommand]
    public async Task ChangePassword(CancellationToken cancellationToken)
    {

    }
}