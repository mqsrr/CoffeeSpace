using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.WebApiClients;

namespace CoffeeSpace.Client.Views;

public partial class LoginView : ContentPage
{
    private readonly ProfileViewModel _profileViewModel;
    private readonly IBuyersWebApi _buyersWebApi;

    public LoginView(LoginViewModel viewModel, ProfileViewModel profileViewModel, IBuyersWebApi buyersWebApi)
    {
        InitializeComponent();
        BindingContext = viewModel;

        _profileViewModel = profileViewModel;
        _buyersWebApi = buyersWebApi;
    }

    protected override async void OnNavigatingFrom(NavigatingFromEventArgs args)
    {

        string buyerEmail = await SecureStorage.GetAsync("buyer-email");
        var buyer = await _buyersWebApi.GetBuyerByEmailAsync(buyerEmail, CancellationToken.None);
        if (buyer == null)
        {
            await Shell.Current.DisplayAlert("Buyer cannot be found!", $"Buyer with {buyerEmail} does not exists", "Ok");
        }

        _profileViewModel.Buyer = buyer;
        await SecureStorage.SetAsync("buyer-id", buyer.Id);

        base.OnNavigatingFrom(args);
    }
}