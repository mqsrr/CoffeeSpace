namespace CoffeeSpace.Messages.Payment.Commands;

public interface PaymentPageInitialized
{
    Guid OrderId { get; }
    
    Guid BuyerId { get; }
    
    string PaymentApprovalLink { get; }
}
