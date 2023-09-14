using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.WebApiClients;
using CommunityToolkit.Maui.Core.Extensions;

namespace CoffeeSpace.Client.Views;

public partial class MainView : ContentPage
{
    private readonly MainViewModel _mainViewModel;
    private readonly IProductsWebApi _productsWebApi;
    
    public MainView(MainViewModel viewModel, IProductsWebApi productsWebApi)
    {
        InitializeComponent();
        BindingContext = viewModel;

        _mainViewModel = viewModel;
        _productsWebApi = productsWebApi;
    }

    protected override async void OnAppearing()
    {
        var products = await _productsWebApi.GetAllProductsAsync(CancellationToken.None);
        _mainViewModel.Products = products.Items.ToObservableCollection();
    }
}