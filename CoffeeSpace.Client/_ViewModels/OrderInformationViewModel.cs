using CoffeeSpace.Client.Messages.Commands;
using CoffeeSpace.Client.Models.Ordering;
using CoffeeSpace.Client.Contracts.Ordering;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;
using CoffeeSpace.Client.Services.Abstractions;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class OrderInformationViewModel : ObservableObject
{
    private readonly ISender _sender;

    [ObservableProperty]
    private List<OrderItem> _orderItems;

    [ObservableProperty]
    private CreateAddressRequest _address;

    [ObservableProperty]
    private bool _paymentPageInitialized;

    public string ApprovalLink { get; private set; }

    public OrderInformationViewModel(ISender sender, IHubConnectionService hubConnectionService)
    {
        _sender = sender;

        Address = new CreateAddressRequest();

        hubConnectionService.OnOrderPaymentPageInitialized((paymentApprovalLink) =>
        {
            _paymentPageInitialized = true;
            ApprovalLink = paymentApprovalLink;
        });
    }

    [RelayCommand]
    private async Task CreateOrderAsync(CancellationToken cancellationToken)
    {
        string buyerId = await SecureStorage.GetAsync("buyer-id");
        bool result = await _sender.Send(new CreateOrderCommand
        {
            CreateOrderRequest = new CreateOrderRequest
            {
                OrderItems = OrderItems,
                Address = Address,
                Status = (int)OrderStatus.Submitted
            },
            BuyerId = Guid.Parse(buyerId)
        }, cancellationToken);

        if (!result)
        {
            await Shell.Current.DisplayAlert("Order cannot be created", "There was a problem in order creation process", "Ok");
        }
    }

    [RelayCommand]
    private async Task SendToPaymentPageAsync(CancellationToken cancellationToken)
    {
        
    }

}
