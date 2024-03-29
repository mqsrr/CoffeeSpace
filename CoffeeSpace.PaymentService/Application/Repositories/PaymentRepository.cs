using CoffeeSpace.PaymentService.Application.Models;
using CoffeeSpace.PaymentService.Application.Repositories.Abstractions;
using CoffeeSpace.PaymentService.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.PaymentService.Application.Repositories;

internal sealed class PaymentRepository : IPaymentRepository
{
    private readonly IPaymentDbContext _dbContext;

    public PaymentRepository(IPaymentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaypalOrderInformation?> GetPaypalOrderByIdAsync(Guid paypalOrderId, CancellationToken cancellationToken)
    {
        var paypalOrderInformation = await _dbContext.PaypalOrders.FindAsync([paypalOrderId], cancellationToken);
        return paypalOrderInformation;
    }

    public async Task<PaypalOrderInformation?> GetByApplicationOrderIdAsync(Guid applicationOrderId,
        CancellationToken cancellationToken)
    {
        var paypalOrderInformation = await _dbContext.PaypalOrders.FirstOrDefaultAsync(order => 
            order.ApplicationOrderId == applicationOrderId, cancellationToken);

        return paypalOrderInformation;
    }

    public async Task<bool> CreatePaymentAsync(PaypalOrderInformation paypalOrderInformation, CancellationToken cancellationToken)
    {
        await _dbContext.PaypalOrders.AddAsync(paypalOrderInformation, cancellationToken);
        int result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<bool> UpdatePaymentStatusAsync(string orderId, string newOrderStatus ,CancellationToken cancellationToken)
    {
        int result = await _dbContext.Orders.Where(o => o.Id == orderId)
            .ExecuteUpdateAsync(setter =>
                setter.SetProperty(o => o.Status, newOrderStatus), cancellationToken);

        return result > 0;
    }

    public async Task<bool> DeletePaymentByIdAsync(Guid paypalOrderId, CancellationToken cancellationToken)
    {
        int result = await _dbContext.PaypalOrders
            .Where(order => order.Id == paypalOrderId)
            .ExecuteDeleteAsync(cancellationToken);

        return result > 0;
    }
}