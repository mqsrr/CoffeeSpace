using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.WebApiClients;
using SecureStorage = Xamarin.Essentials.SecureStorage;

namespace CoffeeSpace.Client.Views;

public partial class ProfileView : ContentPage
{
    private readonly ProfileViewModel _profileViewModel;
    private readonly IBuyersWebApi _buyersWebApi;
    
    public ProfileView(ProfileViewModel viewModel, IBuyersWebApi buyersWebApi)
    {
        InitializeComponent();
        BindingContext = viewModel;
        
        _profileViewModel = viewModel;
        _buyersWebApi = buyersWebApi;
    }

    protected override async void OnAppearing()
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(await SecureStorage.GetAsync("jwt-token"));
        var email = jwtToken.Claims.Single(claim => claim.Type == ClaimTypes.Email).Value;

        var buyer = await _buyersWebApi.GetBuyerByEmailAsync(email, CancellationToken.None);
        _profileViewModel.Buyer = buyer;
    }
}