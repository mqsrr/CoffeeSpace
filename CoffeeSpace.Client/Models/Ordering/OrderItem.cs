using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeSpace.Client.Models.Ordering;

public sealed partial class OrderItem : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Total))]
    private int _quantity;

    public required string Title { get; init; }

    public required string Description { get; init; }

    public required float UnitPrice { get; init; }

    public required float Discount { get; init; }

    public float Total => Quantity * UnitPrice;
}