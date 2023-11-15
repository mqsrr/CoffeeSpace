namespace CoffeeSpace.Messages.Payment;

public interface PaymentPageInitialized
{
    string OrderId { get; }
    
    string BuyerId { get; }
    
    string PaymentApprovalLink { get; }
}
