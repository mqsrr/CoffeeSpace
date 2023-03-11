using CoffeeSpace.Application.Models.CustomerInfo;
using CoffeeSpace.Application.Models.CustomerInfo.CardInfo;
using CoffeeSpace.Application.Models.Orders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.Application.Context;

public class CustomersDb : IdentityDbContext<Customer>
{

    public CustomersDb(DbContextOptions<CustomersDb> options) : base(options)
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