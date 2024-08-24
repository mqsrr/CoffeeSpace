using Avalonia;
using Avalonia.Input;
using CoffeeSpace.AClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SukiUI;
using SukiUI.Controls;

namespace CoffeeSpace.AClient.Views;

public sealed partial class MainWindow : SukiWindow
{
    public MainWindow()
    {
        InitializeComponent();
        ClientSize = new Size(1250, 680);

        DataContext = App.Services.GetRequiredService<MainWindowViewModel>();
    }
    
    private void OnPrimaryColorChangeClick(object? sender, PointerPressedEventArgs e)
    {
        SukiTheme.GetInstance().SwitchColorTheme();
    }

    private void OnThemeChangeClick(object? sender, PointerPressedEventArgs e)
    {
        SukiTheme.GetInstance().SwitchBaseTheme();
    }
}