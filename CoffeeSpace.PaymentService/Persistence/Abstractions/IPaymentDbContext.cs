using CoffeeSpace.PaymentService.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.PaymentService.Persistence.Abstractions;

public interface IPaymentDbContext
{
    DbSet<PaymentHistory> PaymentHistories { get; init; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}