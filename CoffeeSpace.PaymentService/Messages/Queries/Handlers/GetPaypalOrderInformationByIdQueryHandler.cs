using CoffeeSpace.PaymentService.Models;
using CoffeeSpace.PaymentService.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.PaymentService.Messages.Queries.Handlers;

internal sealed class GetPaypalOrderInformationByIdQueryHandler : IQueryHandler<GetPaypalOrderInformationByOrderIdQuery, PaypalOrderInformation?>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetPaypalOrderInformationByIdQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async ValueTask<PaypalOrderInformation?> Handle(GetPaypalOrderInformationByOrderIdQuery query, CancellationToken cancellationToken)
    {
        var paypalOrderInformation = await _paymentRepository.GetPaypalOrderByIdAsync(query.Id, cancellationToken);
        return paypalOrderInformation;
    }
}