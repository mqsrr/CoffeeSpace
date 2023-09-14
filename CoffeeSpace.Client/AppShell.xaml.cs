using CoffeeSpace.Client.Views;

namespace CoffeeSpace.Client;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(MainView), typeof(MainView));
        Routing.RegisterRoute("Order Information", typeof(OrderInformationView));
    }
}