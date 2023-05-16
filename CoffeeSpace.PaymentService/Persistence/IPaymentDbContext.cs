using CoffeeSpace.PaymentService.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.PaymentService.Persistence;

internal interface IPaymentDbContext
{
    DbSet<PaymentHistory> PaymentHistories { get; init; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}