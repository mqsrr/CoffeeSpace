using CoffeeSpace._ViewModels;

namespace CoffeeSpace.Views;

public partial class OrderView : ContentPage
{
	public OrderView(OrderViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}