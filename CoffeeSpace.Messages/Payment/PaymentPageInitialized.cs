namespace CoffeeSpace.Messages.Payment;

public interface PaymentPageInitialized
{
    string BuyerId { get; }
    
    string PaymentApprovalLink { get; }
}
