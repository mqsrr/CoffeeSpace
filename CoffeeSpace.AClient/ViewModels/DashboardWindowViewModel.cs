using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using AutoBogus;
using CoffeeSpace.AClient.Fakers;
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

    [ObservableProperty]
    private Buyer _buyer;

    public DashboardWindowViewModel(ISender sender, IProductsWebApi productsWebApi)
    {
        Products = new ObservableCollection<Product>();
        _sender = sender;
        _productsWebApi = productsWebApi;
    }

    public DashboardWindowViewModel()
    {
        Products = new ObservableCollection<Product>(AutoFaker.Generate<Product, CoffeeFaker>(10));
        _sender = null!;
        _productsWebApi = null!;
    }

    internal async Task InitializeAsync()
    {
        var productsResponse = await _productsWebApi.GetAllProductsAsync(CancellationToken.None);
        if (!productsResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Failure!", "Products could not be fetched");
            return;
        }
        
        Products.AddRange(productsResponse.Content!);
    }

}