using CoffeeSpace.Auth0;
using CoffeeSpace.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using CoffeeSpace.Data.Models.CustomerInfo;
using Microsoft.AspNetCore.Identity;

namespace CoffeeSpace._ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly Auth0Client _clientAuth0;
    private readonly OidcClient _oidcClient;
    private readonly HttpClient _httpClient;
    private readonly SignInManager<Customer> _signInManager;

    public LoginViewModel(Auth0Client clientAuth0, OidcClient client, SignInManager<Customer> signInManager)
    {
        _clientAuth0 = clientAuth0;
        _oidcClient = client;
        _signInManager = signInManager;

        _httpClient = new HttpClient();
    }

    [RelayCommand]
    private async Task LoginWithOidc()
    {
        var loginResult = await _oidcClient.LoginAsync();

        if (loginResult.IsError)
        {
            await Shell.Current.DisplayAlert("Login Error", loginResult.ErrorDescription, "Ok");
            return;
        }

        _httpClient.SetBearerToken(loginResult.AccessToken);
        var response = await _httpClient.GetAsync("https://demo.duendesoftware.com/api/test");

        if (!response.IsSuccessStatusCode)
            await Shell.Current.DisplayAlert("Login Error", loginResult.ErrorDescription, "Ok");
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task LoginWithAuth0()
    {
        var loginResult = await _clientAuth0.LoginAsync();

        await Task.WhenAny(loginResult.IsError
            ? Shell.Current.DisplayAlert("Login Error", loginResult.ErrorDescription, "Ok")
            : Shell.Current.GoToAsync(nameof(MainView)));
    }
}

