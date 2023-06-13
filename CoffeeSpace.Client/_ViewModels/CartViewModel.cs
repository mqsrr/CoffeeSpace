using System.Collections.ObjectModel;
using CoffeeSpace.Client.Models.Products;
using CoffeeSpace.Client.WebApiClients;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class CartViewModel : ObservableObject
{
    private readonly ISender _sender;
    private readonly IBuyersWebApi _buyersWebApi;

    [ObservableProperty]
    private ICollection<Product> _products;
    
    public CartViewModel(ISender sender, IBuyersWebApi buyersWebApi)
    {
        _sender = sender;
        _buyersWebApi = buyersWebApi;
    }

    [RelayCommand]
    private void ClearCart()
    {
        Products.AsParallel().ForAll(x => x.Quantity = 0);
        Products.Clear();
    }

    [RelayCommand]
    internal void AddProductToCart(Product product)
    {
        product.Quantity++;
        if (!Products.Contains(product))
        {
            Products.Add(product);
            return;
        }

        RefreshOrderItem(product);
    }

     [RelayCommand]
     private async Task CreateOrder(CancellationToken cancellationToken)
     {
         
         
         ClearCart();
     }

    private void RefreshOrderItem(Product product)
    {
        Products.Remove(product);
        Products.Add(product);
    }
}