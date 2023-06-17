using CoffeeSpace.Client._ViewModels;

namespace CoffeeSpace.Client.Views;

public partial class OrderView : ContentPage
{
    private readonly ProfileViewModel _profileViewModel;
    private readonly OrderViewModel _orderViewModel;

    public OrderView(OrderViewModel viewModel, ProfileViewModel profileViewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
        _orderViewModel = viewModel;
        
        _profileViewModel = profileViewModel;
    }

    protected override void OnAppearing()
    {
        _orderViewModel.Orders = _profileViewModel.Buyer.Orders.ToList();
    }
}