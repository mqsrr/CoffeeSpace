using CoffeeSpace.Client._ViewModels;

namespace CoffeeSpace.Client.Views;

public partial class OrderView : ContentPage
{
    public OrderView(OrderViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }
}