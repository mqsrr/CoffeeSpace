using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoBogus;
using CoffeeSpace.AClient.Contracts.Requests.Orders;
using CoffeeSpace.AClient.Mappers;
using CoffeeSpace.AClient.Messages.Commands;
using CoffeeSpace.AClient.Models;
using CoffeeSpace.AClient.RefitClients;
using CoffeeSpace.AClient.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;

namespace CoffeeSpace.AClient.ViewModels;

public sealed partial class CartWindowViewModel : ViewModelBase
{
    private readonly IOrderingWebApi _orderingWebApi;
    private readonly ISender _sender;

    [ObservableProperty] 
    private ObservableCollection<Product> _cartProducts;

    [ObservableProperty]
    private CreateAddressRequest _address = new CreateAddressRequest();

    public CartWindowViewModel(IOrderingWebApi orderingWebApi, ISender sender)
    {
        _orderingWebApi = orderingWebApi;
        _sender = sender;
        
        CartProducts = new ObservableCollection<Product>(AutoFaker.Generate<Product>(5));
    }

    public CartWindowViewModel()
    {
        _sender = null!;
        _orderingWebApi = null!;
        
        CartProducts = new ObservableCollection<Product>(AutoFaker.Generate<Product>(5));
    }

    [RelayCommand]
    private async Task CreateOrder(CancellationToken cancellationToken)
    {
        string? buyerId = StaticStorage.BuyerId;
        var orderResponse = await _orderingWebApi.CreateOrder(Guid.Parse(buyerId!), new CreateOrderRequest
        {
            Address = Address,
            OrderItems = CartProducts.Select(product => product.ToOrderItem()),
            Status = 0
        }, cancellationToken);

        await _sender.Send(new CreateOrderHistoryCommand
        {
            Order = orderResponse
        }, cancellationToken);
        
        CartProducts.Clear();
    }
    
    [RelayCommand]
    private Task RemoveFromCart(Product product)
    {
        CartProducts.Remove(product);
        product.Quantity = 1;
        
        return Task.CompletedTask;
    }
}
