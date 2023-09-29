using CoffeeSpace.Client._ViewModels;

namespace CoffeeSpace.Client.Views;

public partial class CartView
{
    public CartView(CartViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}