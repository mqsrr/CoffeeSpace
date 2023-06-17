using System.Collections.ObjectModel;
using CoffeeSpace.Client.Messages.Commands;
using CoffeeSpace.Client.Models.Products;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class MainViewModel : ObservableObject
{
    private readonly ISender _sender;

    [ObservableProperty] 
    private IEnumerable<Product> _products;

    public MainViewModel(ISender sender)
    {
        _sender = sender;

        _products = new List<Product>();
    }

    [RelayCommand]
    private async Task AddToCart(Product product, CancellationToken cancellationToken)
    {
        await _sender.Send(new AddProductToCartCommand
        {
            Product = product
        }, cancellationToken);
    }
}