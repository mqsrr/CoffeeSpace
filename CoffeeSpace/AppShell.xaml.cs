using CoffeeSpace.Views;

namespace CoffeeSpace;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		Routing.RegisterRoute(nameof(MainView), typeof(MainView));
	}
}
