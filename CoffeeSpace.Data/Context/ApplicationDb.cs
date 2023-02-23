using System.Reflection;
using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.Data.Models.CustomerInfo.CardInfo;
using CoffeeSpace.Data.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.Data.Context;

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