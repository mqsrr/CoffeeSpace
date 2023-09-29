using CoffeeSpace.Client.Contracts.Authentication;
using CoffeeSpace.Client.Services.Abstractions;
using CoffeeSpace.Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    [ObservableProperty] 
    private LoginRequest _loginRequest;

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
        _loginRequest = new LoginRequest();
    }

    [RelayCommand]
    private async Task AuthenticateAsync(CancellationToken cancellationToken)
    {
        bool isSuccess = await _authService.LoginAsync(LoginRequest, cancellationToken);
        if (isSuccess is false)
        {
            await Shell.Current.DisplayAlert("Authentication error!", "Please check your credentials and try again.", "Ok"); ;
            return;
        }

        await Shell.Current.GoToAsync(nameof(MainView));
    }

    [RelayCommand]
    private async Task GoToRegisterFormAsync(CancellationToken cancellation)
    {
        await Shell.Current.GoToAsync(nameof(RegisterView));
    }
}