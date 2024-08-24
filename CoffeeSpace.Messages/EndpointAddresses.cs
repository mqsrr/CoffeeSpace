namespace CoffeeSpace.Messages;

public static class EndpointAddresses
{
    public class Identity
    {
        public const string RegisterNewBuyer = "queue:register-new-buyer";
        
        public const string DeleteBuyer = "queue:delete-buyer";
        
        public const string UpdateBuyer = "queue:update-buyer";
    }
    
    public class Ordering
    {
        public const string SubmitOrder = "queue:order-state-instance";
    }
    
    public class Payment
    {
        public const string PaymentPageInitialized = "queue:payment-page-initialized";
        
        public const string OrderPaid = "queue:order-state-instance";
    }
}