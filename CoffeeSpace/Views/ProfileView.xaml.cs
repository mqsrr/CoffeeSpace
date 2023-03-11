using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeSpace._ViewModels;

namespace CoffeeSpace.Views;

public partial class ProfileView : ContentPage
{
    public ProfileView(ProfileViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }

}