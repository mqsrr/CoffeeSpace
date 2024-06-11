using Avalonia.Controls;
using Avalonia.Input;
using CoffeeSpace.AClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeSpace.AClient.Views;

public sealed partial class RegisterWindow : Window
{
    public RegisterWindow()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<RegisterViewModel>();
    }

    private void MoveToLoginWindow(object? sender, PointerPressedEventArgs e)
    {
        Close();
    }
}