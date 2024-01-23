using Avalonia.Controls;
using CoffeeSpace.AClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeSpace.AClient.Views;

public sealed partial class FindMyCoffeeView : UserControl
{
    public FindMyCoffeeView()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<FindMyCoffeeViewModel>();
    }
}