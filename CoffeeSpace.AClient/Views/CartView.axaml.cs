using System;
using System.Threading;
using Avalonia.Controls;
using CoffeeSpace.AClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeSpace.AClient.Views;

public sealed partial class CartView : UserControl
{
    private readonly CartWindowViewModel _viewModel;
    private readonly PaymentView _paymentView;
    
    public CartView()
    {
        InitializeComponent();
        _viewModel = App.Services.GetRequiredService<CartWindowViewModel>();
        DataContext = _viewModel;
        
        _paymentView = new PaymentView();
        _paymentView.Closed += (_, _) => Console.WriteLine("ES");
        
    }
    
    protected override async void OnInitialized()
    {   
        await _viewModel.InitializeAsync(CancellationToken.None);
    }

}