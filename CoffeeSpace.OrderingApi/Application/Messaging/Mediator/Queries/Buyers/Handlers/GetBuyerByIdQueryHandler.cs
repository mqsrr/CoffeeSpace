using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.CustomerInfo;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Queries.Buyers.Handlers;

internal sealed class GetBuyerByIdQueryHandler : IQueryHandler<GetBuyerByIdQuery, Buyer?>
{
    private readonly IBuyerRepository _buyerRepository;

    public GetBuyerByIdQueryHandler(IBuyerRepository buyerRepository)
    {
        _buyerRepository = buyerRepository;
    }

    public async ValueTask<Buyer?> Handle(GetBuyerByIdQuery query, CancellationToken cancellationToken)
    {
        var buyer = await _buyerRepository.GetByIdAsync(query.Id, cancellationToken);

        return buyer;
    }
}