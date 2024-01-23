using Avalonia.Controls;
using CoffeeSpace.AClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeSpace.AClient.Views;

public sealed partial class DashboardView : UserControl
{
    private readonly DashboardWindowViewModel _viewModel;
    
    public DashboardView()
    {
        InitializeComponent();
        
        _viewModel = App.Services.GetRequiredService<DashboardWindowViewModel>();
        DataContext = _viewModel;
    }

    protected override async void OnInitialized()
    {
        await _viewModel.InitializeAsync();
    }
}