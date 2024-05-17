using System;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using CoffeeSpace.AClient.Models;
using CoffeeSpace.AClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;

namespace CoffeeSpace.AClient.Views;

public sealed partial class OrderHistoryView : UserControl
{
    private readonly OrderHistoryWindowViewModel _historyWindowViewModel;
    
    public OrderHistoryView()
    {
        InitializeComponent();
        
        _historyWindowViewModel = App.Services.GetRequiredService<OrderHistoryWindowViewModel>();
        DataContext = _historyWindowViewModel;
    }

    protected override async void OnInitialized()
    {
        await _historyWindowViewModel.InitializeAsync(CancellationToken.None);
    }

    private void InputElement_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        var tappedGlassCard = sender as GlassCard;
        var tappedOrder = tappedGlassCard!.DataContext as Order;
        var orderDetails = new OrderDetailsView(tappedOrder!);
        
        SukiHost.ShowDialog(orderDetails, allowBackgroundClose: true);
    }
}