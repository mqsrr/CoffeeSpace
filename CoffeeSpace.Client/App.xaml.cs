using CoffeeSpace.Client.Contracts.Authentication;
using CoffeeSpace.Client.Views;

namespace CoffeeSpace.Client;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}