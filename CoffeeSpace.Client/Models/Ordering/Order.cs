using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeSpace.Client.Models.Ordering;

public sealed partial class Order : ObservableObject
{
    [ObservableProperty]
    private OrderStatus _status;
    
    [ObservableProperty]
    private TimeSpan _timeToProceedToPayment;

    [ObservableProperty]
    private string _paymentApprovalLink;
    
    public required string Id { get; init; }

    public required Address Address { get; init; }

    public required IEnumerable<OrderItem> OrderItems { get; init; }
}
