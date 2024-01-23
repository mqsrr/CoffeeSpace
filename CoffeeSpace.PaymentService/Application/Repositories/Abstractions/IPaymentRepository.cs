using CoffeeSpace.PaymentService.Application.Models;

namespace CoffeeSpace.PaymentService.Application.Repositories.Abstractions;

public interface IPaymentRepository
{
    Task<PaypalOrderInformation?> GetPaypalOrderByIdAsync(string paypalOrderId, CancellationToken cancellationToken);
    
    Task<PaypalOrderInformation?> GetByApplicationOrderIdAsync(string applicationOrderId, CancellationToken cancellationToken);
    
    Task<bool> CreatePaymentAsync(PaypalOrderInformation paypalOrderInformation, CancellationToken cancellationToken);
    
    Task<bool> UpdatePaymentStatusAsync(string orderId, string newOrderStatus, CancellationToken cancellationToken);

    Task<bool> DeletePaymentByIdAsync(string paypalOrderId, CancellationToken cancellationToken);

}