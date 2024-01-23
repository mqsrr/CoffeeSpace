using System.Reflection;
using CoffeeSpace.PaymentService.Application.Models;
using CoffeeSpace.PaymentService.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;

namespace CoffeeSpace.PaymentService.Persistence;

internal sealed class PaymentDbContext : DbContext, IPaymentDbContext
{
    public required DbSet<PaypalOrderInformation> PaypalOrders { get; init; }

    public required DbSet<Order> Orders { get; init; }

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}