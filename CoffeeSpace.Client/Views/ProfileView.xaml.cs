using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.WebApiClients;

namespace CoffeeSpace.Client.Views;

public partial class ProfileView : ContentPage
{
    
    public ProfileView(ProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}