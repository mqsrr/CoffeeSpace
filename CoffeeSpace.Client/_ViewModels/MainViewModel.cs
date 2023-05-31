using System.Collections.ObjectModel;
using CoffeeSpace.Client.Contracts.Products;
using CoffeeSpace.Client.Messages.Commands;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class MainViewModel : ObservableObject
{
    private readonly ISender _sender;

    [ObservableProperty] 
    private ObservableCollection<ProductResponse> _products;

    public MainViewModel(ISender sender)
    {
        _sender = sender;
    }

    [RelayCommand]
    private async Task AddToCart(ProductResponse product, CancellationToken cancellationToken)
    {
        await _sender.Send(new AddProductToCartCommand
        {
            Product = product
        }, cancellationToken);
    }
}