using Avalonia;
using Avalonia.Controls;
using CoffeeSpace.AClient.ViewModels;

namespace CoffeeSpace.AClient.Views;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ClientSize = new Size(1250, 680);

        DataContext = new MainWindowViewModel();
    }
}