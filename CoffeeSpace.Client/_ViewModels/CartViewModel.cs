using CoffeeSpace.Client.Models.Products;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class CartViewModel : ObservableObject
{
    private readonly ISender _sender;

    [ObservableProperty]
    private ICollection<Product> _products;
    
    public CartViewModel(ISender sender)
    {
        _sender = sender;

        _products = new List<Product>();
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

        RefreshProduct(product);
    }
    
    [RelayCommand]
    private void ClearCart()
    {
        foreach (var product in Products)
        {
            product.Quantity = 0;
        }

        Products.Clear();
    }
    
    private void RefreshProduct(Product product)
    {
        Products.Remove(product);
        Products.Add(product);
    }
}