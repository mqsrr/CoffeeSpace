using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using CoffeeSpace.AClient.Contracts.Requests.Orders;
using CoffeeSpace.AClient.Mappers;
using CoffeeSpace.AClient.Models;
using CoffeeSpace.AClient.RefitClients;
using CoffeeSpace.AClient.Services;
using CoffeeSpace.AClient.Services.Abstractions;
using CoffeeSpace.AClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;

namespace CoffeeSpace.AClient.ViewModels;

public sealed partial class CartWindowViewModel : ViewModelBase
{
    private readonly IOrderingWebApi _orderingWebApi;
    private readonly IHubConnectionService _hubConnectionService;
    private readonly OrderHistoryWindowViewModel _historyWindowViewModel;
    
    [ObservableProperty] 
    private ObservableCollection<Product> _cartProducts;

    [ObservableProperty]
    private CreateAddressRequest _address = new CreateAddressRequest();

    public CartWindowViewModel(IOrderingWebApi orderingWebApi, IHubConnectionService hubConnectionService, OrderHistoryWindowViewModel historyWindowViewModel)
    {
        _orderingWebApi = orderingWebApi;
        _hubConnectionService = hubConnectionService;
        _historyWindowViewModel = historyWindowViewModel;

        CartProducts = new ObservableCollection<Product>();
    }

    public CartWindowViewModel(OrderHistoryWindowViewModel historyWindowViewModel)
    {
        _historyWindowViewModel = historyWindowViewModel;
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
        
        _hubConnectionService.OnOrderCreated(order => 
            Dispatcher.UIThread.Post(async () =>
            {   
                _historyWindowViewModel.Orders.Add(order);
                ClearCartValues();

                await SukiHost.ShowToast("Order Has Been Created", "You will receive payment request in a few seconds.");
            }));
        
        _hubConnectionService.OnOrderPaymentPageInitialized((_, paymentUri) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                var paymentView = App.Services.GetRequiredService<PaymentView>();
                paymentView.WebView.Url = new Uri(paymentUri);
                paymentView.WebView.NavigationStarting += async (_, arg) =>
                {
                    if (arg.Url!.Port == 8085)
                    {
                        await SukiHost.ShowToast("Success", "Order has been paid!");
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
        
    }
    
    [RelayCommand]
    private Task RemoveFromCart(Product product)
    {
        CartProducts.Remove(product);
        product.Quantity = 1;
        
        return Task.CompletedTask;
    }

    private void ClearCartValues()
    {
        CartProducts.Clear();

        Address.Country = string.Empty;
        Address.City = string.Empty;
        Address.Street = string.Empty;
        
        var cartView = App.Services.GetRequiredService<CartView>();
        cartView.GetControl<TextBox>("CountryTextBox").Clear();
        cartView.GetControl<TextBox>("CityTextBox").Clear();
        cartView.GetControl<TextBox>("StreetTextBox").Clear();
    }
}
