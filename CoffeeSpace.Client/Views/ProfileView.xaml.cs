using CoffeeSpace.Client._ViewModels;

namespace CoffeeSpace.Client.Views;

public partial class ProfileView : ContentPage
{
    public ProfileView(ProfileViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}