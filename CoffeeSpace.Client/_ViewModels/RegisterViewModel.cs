using CoffeeSpace.Client.Contracts.Authentication;
using CoffeeSpace.Client.Services.Abstractions;
using CoffeeSpace.Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeSpace.Client._ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    [ObservableProperty]
    private RegisterRequest _registerRequest;

    private readonly IAuthService _authService;

    public RegisterViewModel(IAuthService authService)
    {
        RegisterRequest = new RegisterRequest();
        _authService = authService;
    }

    [RelayCommand]
    private async Task RegisterAsync(CancellationToken cancellationToken)
    {
        bool isSuccess = await _authService.RegisterAsync(RegisterRequest, cancellationToken);
        if (isSuccess is false)
        {
            await Shell.Current.DisplayAlert("Authentication error!", "Please check your credentials and try again.", "Ok");
            return;
        }

        await Shell.Current.GoToAsync(nameof(MainView));
    }

    [RelayCommand]
    private async Task GoToLoginFormAsync(CancellationToken cancellation)
    {
        await Shell.Current.GoToAsync(nameof(LoginView));
    }
}
