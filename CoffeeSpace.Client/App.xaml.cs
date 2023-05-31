using CoffeeSpace.Client.Views;

namespace CoffeeSpace.Client;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App(LoginView loginView)
    {
        InitializeComponent();

        MainPage = new NavigationPage(loginView);
    }
}