using CoffeeSpace._ViewModels;

namespace CoffeeSpace.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}