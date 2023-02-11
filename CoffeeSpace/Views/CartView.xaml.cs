using CoffeeSpace._ViewModels;

namespace CoffeeSpace.Views;

public partial class CartView : ContentPage
{
	public CartView(CartViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}