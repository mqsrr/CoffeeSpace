using CoffeeSpace.PaymentService.Models;

namespace CoffeeSpace.PaymentService.Repositories.Abstractions;

internal interface IPaymentHistoryRepository
{
    Task<IEnumerable<PaymentHistory>> GetAllAsync(CancellationToken cancellationToken);
    
    Task<PaymentHistory?> GetByIdAsync(string id, CancellationToken cancellationToken);
    
    Task<bool> CreateAsync(PaymentHistory paymentHistory, CancellationToken cancellationToken);
    
    Task<PaymentHistory?> UpdateAsync(PaymentHistory paymentHistory, CancellationToken cancellationToken);
    
    Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken);
}