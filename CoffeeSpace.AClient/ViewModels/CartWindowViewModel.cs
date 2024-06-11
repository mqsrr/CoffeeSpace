using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CoffeeSpace.AClient.Contracts.Requests.Orders;
using CoffeeSpace.AClient.Mappers;
using CoffeeSpace.AClient.Messages.Commands;
using CoffeeSpace.AClient.Models;
using CoffeeSpace.AClient.RefitClients;
using CoffeeSpace.AClient.Services;
using CoffeeSpace.AClient.Services.Abstractions;
using CoffeeSpace.AClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;
using SukiUI.Controls;

namespace CoffeeSpace.AClient.ViewModels;

public sealed partial class CartWindowViewModel : ViewModelBase
{
    private readonly IOrderingWebApi _orderingWebApi;
    private readonly ISender _sender;
    private readonly IHubConnectionService _hubConnectionService;
    
    [ObservableProperty] 
    private ObservableCollection<Product> _cartProducts;

    [ObservableProperty]
    private CreateAddressRequest _address = new CreateAddressRequest();

    public CartWindowViewModel(IOrderingWebApi orderingWebApi, ISender sender, IHubConnectionService hubConnectionService)
    {
        _orderingWebApi = orderingWebApi;
        _sender = sender;
        _hubConnectionService = hubConnectionService;
        
        CartProducts = new ObservableCollection<Product>();
    }

    public CartWindowViewModel()
    {
        _sender = null!;
        _orderingWebApi = null!;
        _hubConnectionService = null!;
        
        CartProducts = new ObservableCollection<Product>();
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (!_hubConnectionService.IsConnected)
        {
            await _hubConnectionService.StartConnectionAsync(StaticStorage.Buyer!.Id, cancellationToken);
        }
        
        _hubConnectionService.OnOrderCreated(_ => 
            Dispatcher.UIThread.Post(async () => 
                await SukiHost.ShowToast("Success", "Order has been created")));
        
        _hubConnectionService.OnOrderPaymentPageInitialized((_, paymentUri) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                var paymentView = new PaymentView();
                paymentView.WebView.Url = new Uri(paymentUri);
                paymentView.WebView.WebMessageReceived += (_, args) =>
                {
                    if (args.Source.Port == 8085)
                    {
                        paymentView.Close();
                    }
                };
                paymentView.Show();
            });
        });
    }
    
    [RelayCommand]
    private async Task CreateOrder(CancellationToken cancellationToken)
    {
        string buyerId = StaticStorage.Buyer!.Id;
        var createdOrderResponse = await _orderingWebApi.CreateOrder(Guid.Parse(buyerId), new CreateOrderRequest
        {
            Address = Address,
            OrderItems = CartProducts.Select(product => product.ToOrderItemRequest()),
            Status = 0
        }, cancellationToken);

        if (!createdOrderResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Failure", "We couldn't create order. Please try again!");
            return;
        }
        
        await _sender.Send(new CreateOrderHistoryCommand
        {
            Order = createdOrderResponse.Content!
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
