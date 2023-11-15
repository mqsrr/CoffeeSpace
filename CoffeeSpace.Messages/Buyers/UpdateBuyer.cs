using CoffeeSpace.Domain.Ordering.BuyerInfo;

namespace CoffeeSpace.Messages.Buyers;

public interface UpdateBuyer
{
    public Buyer Buyer { get; }
}