using System.Reflection;
using CoffeeSpace.PaymentService.Models;
using CoffeeSpace.PaymentService.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.PaymentService.Persistence;

internal sealed class PaymentDbContext : DbContext, IPaymentDbContext
{
    public required DbSet<PaymentHistory> PaymentHistories { get; init; }
    
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}