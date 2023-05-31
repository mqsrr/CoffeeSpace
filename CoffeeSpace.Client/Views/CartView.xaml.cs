using CoffeeSpace.Client._ViewModels;

namespace CoffeeSpace.Client.Views;

public partial class CartView : ContentPage
{
    public CartView(CartViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}