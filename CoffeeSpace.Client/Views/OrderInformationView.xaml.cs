using CoffeeSpace.Client._ViewModels;

namespace CoffeeSpace.Client.Views;

public partial class OrderInformationView
{
	public OrderInformationView(OrderInformationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}