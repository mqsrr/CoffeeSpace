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

    [RelayCommand]
    private async Task RegisterAsync(CancellationToken cancellationToken)
    {
        try
        {
            string? token = await _identityWebApi.RegisterAsync(RegisterRequest, cancellationToken);
            if (string.IsNullOrEmpty(token))
            {
                return;
            }
            
            var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            
            string email = tokenHandler.Claims.First(claim => claim.Type is ClaimTypes.Email).Value;
            var buyer = await _buyersWebApi.GetBuyerByEmailAsync(email, cancellationToken);
            
            StaticStorage.JwtToken = token;
            StaticStorage.Buyer = buyer;
            LoginViewModel.MoveToMainDashBoard();
        }
        catch (Exception e)
        {
            //ignored
        }
    }
}