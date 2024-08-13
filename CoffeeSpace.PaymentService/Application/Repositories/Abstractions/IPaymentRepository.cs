using CoffeeSpace.PaymentService.Application.Models;

namespace CoffeeSpace.PaymentService.Application.Repositories.Abstractions;

public interface IPaymentRepository
{
    Task<PaypalOrderInformation?> GetPaypalOrderByIdAsync(Guid paypalOrderId, CancellationToken cancellationToken);
    
    Task<PaypalOrderInformation?> GetByApplicationOrderIdAsync(Guid applicationOrderId, CancellationToken cancellationToken);
    
    Task<bool> CreatePaymentAsync(PaypalOrderInformation paypalOrderInformation, CancellationToken cancellationToken);
    
    Task<bool> UpdatePaymentStatusAsync(string orderId, string orderStatus, CancellationToken cancellationToken);
}