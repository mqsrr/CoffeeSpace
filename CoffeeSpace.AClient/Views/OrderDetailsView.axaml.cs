using AutoBogus;
using Avalonia.Controls;
using CoffeeSpace.AClient.Models;
using CoffeeSpace.AClient.ViewModels;

namespace CoffeeSpace.AClient.Views;

public partial class OrderDetailsView : UserControl
{
    public OrderDetailsView(Order order)
    {
        InitializeComponent();
        DataContext = new OrderDetailsViewModel(order);
    }

    public OrderDetailsView()
    {
        InitializeComponent();
        DataContext = new OrderDetailsViewModel(AutoFaker.Generate<Order>());
    }
}