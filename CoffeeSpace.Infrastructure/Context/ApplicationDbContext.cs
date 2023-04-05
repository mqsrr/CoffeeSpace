using System.Reflection;
using CoffeeSpace.Domain.Models.CustomerInfo;
using CoffeeSpace.Domain.Models.CustomerInfo.CardInfo;
using CoffeeSpace.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.Infrastructure.Context;

public sealed class ApplicationDbContext : DbContext
{
    public required DbSet<OrderItem> OrderItems { get; init; }
    
    public required DbSet<Order> Orders { get; init; }
    
    public required DbSet<Customer> Customers { get; init; }
    
    public required DbSet<Address> Addresses { get; init; }
    
    public required DbSet<PaymentInfo> PaymentInfos { get; init; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
                
        base.OnModelCreating(modelBuilder);
    }
}