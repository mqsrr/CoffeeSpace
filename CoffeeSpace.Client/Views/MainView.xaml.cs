using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.WebApiClients;

namespace CoffeeSpace.Client.Views;

public partial class MainView : ContentPage
{
    private readonly IProductsWebApi _productsWebApi;
    
    public MainView(MainViewModel viewModel, IProductsWebApi productsWebApi)
    {
        InitializeComponent();

        BindingContext = viewModel;
        _productsWebApi = productsWebApi;
    }

    protected override async void OnAppearing()
    {
        var products = await _productsWebApi.GetAllProductsAsync(CancellationToken.None);
        var viewModel = BindingContext as MainViewModel;

        viewModel!.Products = products;
    }
}