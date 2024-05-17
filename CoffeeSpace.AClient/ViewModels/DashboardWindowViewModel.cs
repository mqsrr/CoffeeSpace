using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using AutoBogus;
using CoffeeSpace.AClient.Messages.Commands;
using CoffeeSpace.AClient.Models;
using CoffeeSpace.AClient.RefitClients;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Mediator;
using SukiUI.Controls;

namespace CoffeeSpace.AClient.ViewModels;

public sealed partial class DashboardWindowViewModel : ViewModelBase
{
    private readonly ISender _sender;
    private readonly IProductsWebApi _productsWebApi;

    [ObservableProperty]
    private ObservableCollection<Product> _products;

    public DashboardWindowViewModel(ISender sender, IProductsWebApi productsWebApi)
    {
        Products = new ObservableCollection<Product>();
        _sender = sender;
        _productsWebApi = productsWebApi;
    }

    public DashboardWindowViewModel()
    {
        Products = new ObservableCollection<Product>(AutoFaker.Generate<Product>(10));
        _sender = null!;
        _productsWebApi = null!;
    }

    internal async Task InitializeAsync()
    {
        var products = await _productsWebApi.GetAllProductsAsync(CancellationToken.None);
        Products.AddRange(products);
    }
    
    [RelayCommand]
    private async Task AddToCart(Product product, CancellationToken cancellationToken)
    {
        await _sender.Send(new AddToCartCommand
        {
            Product = product
        }, cancellationToken);

        await SukiHost.ShowToast("Cart is updated", "Item has been added!", TimeSpan.FromSeconds(5));
    }
}