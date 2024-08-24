using System.Threading;
using Avalonia.Controls;
using CoffeeSpace.AClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeSpace.AClient.Views;

public sealed partial class CartView : UserControl
{
    private readonly CartWindowViewModel _viewModel;
    
    public CartView()
    {
        InitializeComponent();
        _viewModel = App.Services.GetRequiredService<CartWindowViewModel>();
        DataContext = _viewModel;
    }

    protected override async void OnInitialized()
    {   
        await _viewModel.InitializeAsync(CancellationToken.None);
    }
}