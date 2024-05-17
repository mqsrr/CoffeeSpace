using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CoffeeSpace.AClient.Contracts.Requests.Auth;
using CoffeeSpace.AClient.RefitClients;
using CoffeeSpace.AClient.Services;
using CoffeeSpace.AClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeSpace.AClient.ViewModels;

public sealed partial class LoginViewModel : ViewModelBase
{
    private readonly IIdentityWebApi _identityWebApi;
    private readonly IBuyersWebApi _buyersWebApi;

    [ObservableProperty]
    private LoginRequest _loginRequest = new LoginRequest();

    public LoginViewModel(IIdentityWebApi identityWebApi, IBuyersWebApi buyersWebApi)
    {
        _identityWebApi = identityWebApi;
        _buyersWebApi = buyersWebApi;
    }

    public LoginViewModel()
    {
        _identityWebApi = null!;
        _buyersWebApi = null!;
    }

    [RelayCommand]
    private async Task LoginAsync(CancellationToken cancellationToken)
    {
        try
        {
            string? token = await _identityWebApi.LoginAsync(LoginRequest, cancellationToken);
            if (string.IsNullOrEmpty(token))
            {
                return;
            }
            
            var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string email = tokenHandler.Claims.First(claim => claim.Type is ClaimTypes.Email).Value;
            StaticStorage.JwtToken = token;
            
            var buyer = await _buyersWebApi.GetBuyerByEmailAsync(email, cancellationToken);
            StaticStorage.Buyer = buyer;
            
            MoveToMainDashBoard();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }

    internal static void MoveToMainDashBoard()
    {
        var desktop = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
        var mainWindow = new MainWindow();
        mainWindow.Show();

        desktop.MainWindow!.Close();
        desktop.MainWindow = mainWindow;
    }
}