using CoffeeSpace.Client._ViewModels;
using System.IdentityModel.Tokens.Jwt;
using CoffeeSpace.Client.Messages.Commands;
using Mediator;

namespace CoffeeSpace.Client.Views;

public partial class LoginView
{
    private readonly ISender _sender;
    
    public LoginView(LoginViewModel viewModel, ISender sender)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
        _sender = sender;
    }

    protected override async void OnAppearing()
    {
        string authToken = await SecureStorage.GetAsync("jwt-token");
        if (!IsJwtTokenExpired(authToken))
        {
            await Shell.Current.GoToAsync(nameof(MainView));
        }

        base.OnAppearing();
    }

    protected override async void OnNavigatingFrom(NavigatingFromEventArgs args)
    {

        string buyerEmail = await SecureStorage.GetAsync("buyer-email");
        string token = await SecureStorage.GetAsync("jwt-token");
        if (IsJwtTokenExpired(token))
        {
            return;
        }

        bool result = await _sender.Send(new SetNewGlobalBuyerCommand
        {
            BuyerEmail = buyerEmail
        });
        
        if (!result)
        {
            await Shell.Current.DisplayAlert("Buyer cannot be found!", $"Buyer with {buyerEmail} does not exists", "Ok");
            return;
        }
        base.OnNavigatingFrom(args);
    }

    private static bool IsJwtTokenExpired(string token)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadToken(token);
        return jwtToken is not null && jwtToken.ValidTo < DateTime.UtcNow;
    }
}