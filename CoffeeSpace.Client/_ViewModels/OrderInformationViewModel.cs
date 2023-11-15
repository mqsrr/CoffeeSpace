using CoffeeSpace.Client.Messages.Commands;
using CoffeeSpace.Client.Models.Ordering;
using CoffeeSpace.Client.Contracts.Ordering;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class OrderInformationViewModel : ObservableObject
{
    private readonly ISender _sender;

    [ObservableProperty]
    private List<OrderItem> _orderItems;

    [ObservableProperty]
    private CreateAddressRequest _address = new CreateAddressRequest();
    
    public OrderInformationViewModel(ISender sender)
    {
        _sender = sender;
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
            return;
        }

        await Shell.Current.GoToAsync("..");
    }
}
