using CoffeeSpace.Client._ViewModels;

namespace CoffeeSpace.Client.Views;

public partial class LoginView : ContentPage
{
    public LoginView(LoginViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}