using CoffeeSpace._ViewModels;

namespace CoffeeSpace.Views;

public partial class MainView : ContentPage
{

	public MainView(MainViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

}