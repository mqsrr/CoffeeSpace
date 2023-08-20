using CoffeeSpace.PaymentService.Models;
using CoffeeSpace.PaymentService.Persistence.Abstractions;
using CoffeeSpace.PaymentService.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.PaymentService.Repositories;

internal sealed class PaymentHistoryRepository : IPaymentHistoryRepository
{
    private readonly IPaymentDbContext _paymentDbContext;

    public PaymentHistoryRepository(IPaymentDbContext paymentDbContext)
    {
        _paymentDbContext = paymentDbContext;
    }

    public async Task<IEnumerable<PaymentHistory>> GetAllAsync(CancellationToken cancellationToken)
    {
        bool isNotEmpty = await _paymentDbContext.PaymentHistories.AnyAsync(cancellationToken);
        
        return !isNotEmpty
            ? Enumerable.Empty<PaymentHistory>()
            : _paymentDbContext.PaymentHistories;
    }

    public async Task<PaymentHistory?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var paymentHistory = await _paymentDbContext.PaymentHistories.FindAsync(new object?[] {id}, cancellationToken);
        return paymentHistory;
    }

    public async Task<bool> CreateAsync(PaymentHistory paymentHistory, CancellationToken cancellationToken)
    {
        await _paymentDbContext.PaymentHistories.AddAsync(paymentHistory, cancellationToken);
        var result = await _paymentDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<PaymentHistory?> UpdateAsync(PaymentHistory paymentHistory, CancellationToken cancellationToken)
    {
        var result = await _paymentDbContext.PaymentHistories
            .Where(payment => payment.Id == paymentHistory.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(payment => payment.OrderDate, paymentHistory.OrderDate)
                .SetProperty(payment => payment.TotalPrice, paymentHistory.TotalPrice), cancellationToken);
        
        return result > 0
            ? paymentHistory
            : null;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken)
    {
        int result = await _paymentDbContext.PaymentHistories
            .Where(payment => payment.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return result > 0;
    }
}