using CoffeeSpace.PaymentService.Models;
using CoffeeSpace.PaymentService.Persistence;
using CoffeeSpace.PaymentService.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.PaymentService.Repositories;

internal sealed class PaymentHistoryRepository : IPaymentHistoryRepository
{
    private readonly PaymentDbContext _paymentDbContext;

    public PaymentHistoryRepository(PaymentDbContext paymentDbContext)
    {
        _paymentDbContext = paymentDbContext;
    }

    public async Task<IEnumerable<PaymentHistory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var isNotEmpty = await _paymentDbContext.PaymentHistories.AnyAsync(cancellationToken);
        
        return !isNotEmpty
            ? Enumerable.Empty<PaymentHistory>()
            : _paymentDbContext.PaymentHistories;
    }

    public async Task<PaymentHistory?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var paymentHistory = await _paymentDbContext.PaymentHistories.FindAsync(new object?[] {id}, cancellationToken);
        return paymentHistory;
    }

    public async Task<bool> CreateAsync(PaymentHistory paymentHistory, CancellationToken cancellationToken = default)
    {
        await _paymentDbContext.PaymentHistories.AddAsync(paymentHistory, cancellationToken);
        var result = await _paymentDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<PaymentHistory?> UpdateAsync(PaymentHistory paymentHistory, CancellationToken cancellationToken = default)
    {
        try
        {
            _paymentDbContext.PaymentHistories.Update(paymentHistory);
            await _paymentDbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return null;
        }

        return paymentHistory;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var paymentHistory = await _paymentDbContext.PaymentHistories.FindAsync(new object[] { id }, cancellationToken);
        if (paymentHistory is null)
        {
            return false;
        }
        
        _paymentDbContext.PaymentHistories.Remove(paymentHistory);
        await _paymentDbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}