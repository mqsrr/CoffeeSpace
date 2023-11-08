using CoffeeSpace.Client._ViewModels;

namespace CoffeeSpace.Client.Views;

public partial class OrderPaymentView : ContentPage
{
	public OrderPaymentView(OrderPaymentViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}