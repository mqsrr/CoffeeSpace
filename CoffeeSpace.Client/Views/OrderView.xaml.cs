using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.Services.Abstractions;
using CoffeeSpace.Client.WebApiClients;
using CommunityToolkit.Maui.Core.Extensions;

namespace CoffeeSpace.Client.Views;

public partial class OrderView : ContentPage
{ 
    private readonly IOrderingWebApi _orderingWebApi;
    private readonly OrderViewModel _orderViewModel;
    private bool _isInitialized;
    
    public OrderView(OrderViewModel viewModel, IOrderingWebApi orderingWebApi, OrderViewModel orderViewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        
        _orderingWebApi = orderingWebApi;
        _orderViewModel = orderViewModel;
        _isInitialized = false;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_isInitialized)
        {
            return;
        }

        string buyerId = await SecureStorage.GetAsync("buyer-id");
        var orders = await _orderingWebApi.GetAllOrdersByBuyerId(Guid.Parse(buyerId), CancellationToken.None);

        _orderViewModel.Orders = _orderViewModel.Orders.UnionBy(orders, order => order.Id).ToObservableCollection();
        _isInitialized = true;
    }
}