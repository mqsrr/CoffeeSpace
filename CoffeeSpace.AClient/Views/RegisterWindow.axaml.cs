using Avalonia.Controls;
using Avalonia.Input;
using CoffeeSpace.AClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;

namespace CoffeeSpace.AClient.Views;

public sealed partial class RegisterWindow : Window
{
    public RegisterWindow()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<RegisterViewModel>();
    }
}