using CoffeeSpace.Domain.Models.CustomerInfo;
using CoffeeSpace.Domain.Models.CustomerInfo.CardInfo;
using CoffeeSpace.Domain.Models.Orders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.Infrastructure.Context;

public sealed class CustomersDbContext : IdentityDbContext<Customer>
{
    public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<Order>();
        builder.Ignore<OrderItem>();
        builder.Ignore<Address>();
        builder.Ignore<PaymentInfo>();
        builder.Ignore<Customer>();
        
        base.OnModelCreating(builder);
    }
}