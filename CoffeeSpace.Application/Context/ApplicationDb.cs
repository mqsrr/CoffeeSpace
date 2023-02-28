using System.Reflection;
using CoffeeSpace.Application.Models.CustomerInfo;
using CoffeeSpace.Application.Models.CustomerInfo.CardInfo;
using CoffeeSpace.Application.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.Application.Context;

public class ApplicationDb : DbContext
{
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Address> Addresses { get; set; } = null!;
    public DbSet<PaymentInfo> PaymentInfos { get; set; } = null!;

    public ApplicationDb(DbContextOptions<ApplicationDb> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
                
        base.OnModelCreating(modelBuilder);
    }
}