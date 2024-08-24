using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.Contracts.Requests.Auth;
using CoffeeSpace.AClient.RefitClients;
using CoffeeSpace.AClient.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Controls;

namespace CoffeeSpace.AClient.ViewModels;

public sealed partial class RegisterViewModel : ViewModelBase
{
    private readonly IIdentityWebApi _identityWebApi;
    private readonly IBuyersWebApi _buyersWebApi;

    [ObservableProperty]
    private RegisterRequest _registerRequest = null!;

    public RegisterViewModel(IIdentityWebApi identityWebApi, IBuyersWebApi buyersWebApi)
    {
        _identityWebApi = identityWebApi;
        _buyersWebApi = buyersWebApi;
    }

    public RegisterViewModel()
    {
        _identityWebApi = null!;
        _buyersWebApi = null!;
    }
    
    [RelayCommand]
    private async Task RegisterAsync(CancellationToken cancellationToken)
    {

        var authResponse = await _identityWebApi.RegisterAsync(RegisterRequest, cancellationToken);
        if (!authResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Register Failure", "Could not register an account with given credentials!");
            return;
        }

        string token = authResponse.Content!;
        var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
        
        string email = tokenHandler.Claims.First(claim => claim.Type is ClaimTypes.Email).Value;
        var buyerResponse = await _buyersWebApi.GetBuyerByEmailAsync(email, cancellationToken);
        if (!buyerResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Register Failure", "Could not find an account with given email!");
            return;
        }
        
        StaticStorage.JwtToken = token;
        StaticStorage.Buyer = buyerResponse.Content;
        LoginViewModel.MoveToMainDashBoard();
    }
}