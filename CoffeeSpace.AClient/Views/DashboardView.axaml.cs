using Avalonia.Controls;
using Avalonia.Input;
using CoffeeSpace.AClient.Models;
using CoffeeSpace.AClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;

namespace CoffeeSpace.AClient.Views;

public sealed partial class DashboardView : UserControl
{
    private readonly DashboardWindowViewModel _viewModel;
    private readonly ProductDetailsView _productDetailsView;
    
    public DashboardView()
    {
        InitializeComponent();

        _viewModel = App.Services.GetRequiredService<DashboardWindowViewModel>();
        _productDetailsView = App.Services.GetRequiredService<ProductDetailsView>();
        
        DataContext = _viewModel;
    }

    protected override async void OnInitialized()
    {
        await _viewModel.InitializeAsync();
    }

    private void OpenProductDetailsView(object? sender, TappedEventArgs e)
    {
        var glassCard = sender as GlassCard;
        var selectedProduct = glassCard!.DataContext as Product;
        
        _productDetailsView.SetNewProduct(selectedProduct!);
        SukiHost.ShowDialog(_productDetailsView, allowBackgroundClose: true);
    }
}