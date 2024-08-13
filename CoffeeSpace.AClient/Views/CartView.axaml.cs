using System.Threading;
using Avalonia.Controls;
using CoffeeSpace.AClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;

namespace CoffeeSpace.AClient.Views;

public sealed partial class CartView : UserControl
{
    private readonly CartWindowViewModel _viewModel;
    
    public CartView()
    {
        InitializeComponent();
        _viewModel = App.Services.GetRequiredService<CartWindowViewModel>();
        DataContext = _viewModel;
        
       var paymentView = App.Services.GetRequiredService<PaymentView>();
       paymentView.Closed += async (_, _) => await SukiHost.ShowToast("Success", "Order has been paid!");
    }

    protected override async void OnInitialized()
    {   
        await _viewModel.InitializeAsync(CancellationToken.None);
    }
}