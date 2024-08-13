using System;
using Avalonia.Controls;
using Avalonia.Input;
using CoffeeSpace.AClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;

namespace CoffeeSpace.AClient.Views;

public sealed partial class LoginWindow : SukiWindow
{
    private readonly RegisterWindow _registerView;
    
    public LoginWindow()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<LoginViewModel>();
        _registerView = App.Services.GetRequiredService<RegisterWindow>();
        
        _registerView.Closing += OnClosingRegisterView;
        Closing += (_, _) => _registerView.Closing -= OnClosingRegisterView;
    }
    
    protected override void OnClosed(EventArgs e)
    {
        _registerView.Close();
    }

    private void MoveToRegisterWindow(object? sender, PointerPressedEventArgs e)
    {
        Hide();
        _registerView.Show();
    }

    private void OnClosingRegisterView(object? o, WindowClosingEventArgs args)
    {
        ((Window)o!).Hide();
        args.Cancel = true;
        Show();
    }
}