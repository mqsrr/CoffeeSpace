using CoffeeSpace.Auth0;
using CoffeeSpace.Data.Authentication.Response;
using CoffeeSpace.Dto;
using CoffeeSpace.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using CoffeeSpace.Messages.Requests;
using CoffeeSpace.Services;
using MediatR;

namespace CoffeeSpace._ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly Auth0Client _clientAuth0;
    private readonly OidcClient _oidcClient;
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;
    private readonly ISender _sender;

    public CustomerLoginModel LoginModel { get; set; }
    
    public LoginViewModel(Auth0Client clientAuth0, OidcClient client, IAuthService authService, ISender sender)
    {
        _clientAuth0 = clientAuth0;
        _oidcClient = client;
        _authService = authService;
        _sender = sender;

        _httpClient = new HttpClient();
        LoginModel = new CustomerLoginModel();
    }

    [RelayCommand]
    private async Task Login(CancellationToken cancellationToken)
    {
        JwtResponse response = await _authService.LoginAsync(LoginModel, cancellationToken);

        if (!response.IsSuccess)
        {
            await Shell.Current.DisplayAlert("Invalid customer credentials",
                "Entered username or password is invalid!",
                "OK");
            
            return;
        }
        
        await _sender.Send(new CustomerAuthenticatedRequest(response.Customer), cancellationToken);
        //send mediatr request to authenticate customer in application

        await Shell.Current.GoToAsync("MainView");
    }    
    
    [RelayCommand]
    private async Task Register(CustomerRegisterModel registerModel, CancellationToken cancellationToken)
    {
        JwtResponse response = await _authService.RegisterAsync(registerModel, cancellationToken);

        if (!response.IsSuccess)
        {
            await Shell.Current.DisplayAlert("Invalid customer credentials",
                "Please enter valid customer data in registration fields",
                "OK");
            
            return;
        }
        
        //send mediatr request to authenticate customer in application

        await Shell.Current.GoToAsync("MainPage");
    }

    [RelayCommand]
    private async Task LoginWithOidc(CancellationToken cancellationToken)
    {
        var loginResult = await _oidcClient.LoginAsync(cancellationToken: cancellationToken);

        if (loginResult.IsError)
        {
            await Shell.Current.DisplayAlert("Login Error", loginResult.ErrorDescription, "Ok");
            return;
        }

        _httpClient.SetBearerToken(loginResult.AccessToken);
        var response = await _httpClient.GetAsync("https://demo.duendesoftware.com/api/test", cancellationToken);

        if (!response.IsSuccessStatusCode)
            await Shell.Current.DisplayAlert("Login Error", loginResult.ErrorDescription, "Ok");
    }

    [RelayCommand]
    private async Task LoginWithAuth0(CancellationToken cancellationToken)
    {
        var loginResult = await _clientAuth0.LoginAsync(cancellationToken);

        await Task.WhenAny(loginResult.IsError
            ? Shell.Current.DisplayAlert("Login Error", loginResult.ErrorDescription, "Ok")
            : Shell.Current.GoToAsync(nameof(MainView)));
    }
}

