using CoffeeSpace.Client._ViewModels;

namespace CoffeeSpace.Client.Views;

public partial class OrderInformationView : ContentPage
{


	public OrderInformationView(OrderInformationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}