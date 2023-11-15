using CoffeeSpace.Client.Models.Ordering;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty] 
    private Buyer _buyer;
}