using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.Messages.Queries;
using CommunityToolkit.Maui.Core.Extensions;
using Mediator;

namespace CoffeeSpace.Client.Views;

public partial class MainView
{
    private readonly MainViewModel _mainViewModel;
    private readonly ISender _sender;
    
    public MainView(MainViewModel viewModel, ISender sender)
    {
        InitializeComponent();
        BindingContext = viewModel;

        _mainViewModel = viewModel;
        _sender = sender;
    }


    protected override async void OnAppearing()
    {
        if (_mainViewModel.Products.Count > 0)
        {
            return;
        }

        var products = await _sender.Send(new GetAllProductsQuery());
        _mainViewModel.Products = products.ToObservableCollection();
    }
}