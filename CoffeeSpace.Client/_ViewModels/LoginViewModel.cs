using CoffeeSpace.Client.Contracts.Login;
using CoffeeSpace.Client.Services.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    private LoginRequest _loginRequest = new LoginRequest();

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task Authenticate(CancellationToken cancellationToken)
    {
        var isSuccess = await _authService.LoginAsync(LoginRequest, cancellationToken);
        if (isSuccess is false)
        {
            await Shell.Current.DisplayAlert("Authentication error!", "Please check your credentials and try again.", "Ok");
            return;
        }

        await Shell.Current.GoToAsync("MainPage");
    }
}