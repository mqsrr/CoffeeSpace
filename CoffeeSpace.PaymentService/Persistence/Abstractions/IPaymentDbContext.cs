using CoffeeSpace.PaymentService.Models;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;

namespace CoffeeSpace.PaymentService.Persistence.Abstractions;

public interface IPaymentDbContext
{
    DbSet<PaypalOrderInformation> PaypalOrders { get; init; }
    
    DbSet<Order> Orders { get; init; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}