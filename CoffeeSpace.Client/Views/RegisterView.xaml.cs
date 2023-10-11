using CoffeeSpace.Client._ViewModels;
using System.IdentityModel.Tokens.Jwt;
using CoffeeSpace.Client.Messages.Commands;
using Mediator;
using Microsoft.IdentityModel.Tokens;

namespace CoffeeSpace.Client.Views;

public partial class RegisterView
{
    private readonly ISender _sender;

    public RegisterView(RegisterViewModel viewModel, ISender sender)
    {
        InitializeComponent();

        BindingContext = viewModel;
        _sender = sender;
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
        }
        base.OnNavigatingFrom(args);
    }

    private static bool IsJwtTokenExpired(string token)
    {
        if (token.IsNullOrEmpty())
        {
            return true;
        }

        var jwtToken = new JwtSecurityTokenHandler().ReadToken(token);
        return jwtToken is not null && jwtToken.ValidTo < DateTime.UtcNow;
    }
}