using Avalonia;
using Avalonia.Controls;
using CoffeeSpace.AClient.ViewModels;
using HotAvalonia;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeSpace.AClient.Views;

public sealed partial class CartView : UserControl
{
    public CartView()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<CartWindowViewModel>();
    }
}