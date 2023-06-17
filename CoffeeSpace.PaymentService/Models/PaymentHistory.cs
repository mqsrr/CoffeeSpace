namespace CoffeeSpace.PaymentService.Models;

public sealed class PaymentHistory
{
    public required string Id { get; init; }
    
    public required string OrderId { get; init; }

    public required string PaymentId { get; init; }

    public required DateTime OrderDate { get; init; }

    public required float TotalPrice { get; init; }
}