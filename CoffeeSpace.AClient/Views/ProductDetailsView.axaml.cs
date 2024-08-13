using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CoffeeSpace.AClient.Models;
using CoffeeSpace.AClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeSpace.AClient.Views;

public partial class ProductDetailsView : UserControl
{
    private readonly ProductDetailsViewModel _viewModel;
    
    public ProductDetailsView()
    {
        InitializeComponent();
        _viewModel = App.Services.GetRequiredService<ProductDetailsViewModel>();

        DataContext = _viewModel;
    }

    public void SetNewProduct(Product product)
    {
        _viewModel.Product = product;
    }
}