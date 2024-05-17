using Avalonia;
using CoffeeSpace.AClient.ViewModels;
using SukiUI.Controls;

namespace CoffeeSpace.AClient.Views;

public sealed partial class MainWindow : SukiWindow
{
    public MainWindow()
    {
        InitializeComponent();
        ClientSize = new Size(1250, 680);

        DataContext = new MainWindowViewModel();
    }
}