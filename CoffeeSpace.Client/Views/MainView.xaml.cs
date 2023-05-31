using CoffeeSpace.Client._ViewModels;

namespace CoffeeSpace.Client.Views;

public partial class MainView : ContentPage
{
    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}