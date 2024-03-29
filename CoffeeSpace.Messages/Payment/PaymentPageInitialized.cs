namespace CoffeeSpace.Messages.Payment;

public interface PaymentPageInitialized
{
    Guid OrderId { get; }
    
    Guid BuyerId { get; }
    
    string PaymentApprovalLink { get; }
}
