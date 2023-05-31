using CoffeeSpace.Client.Views;

namespace CoffeeSpace.Client;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("MainPage", typeof(MainView));
    }
}