using CoffeeSpace.Domain.Ordering.CustomerInfo;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Queries.Buyers.Handlers;

internal sealed class GetBuyerByEmailQueryHandler : IQueryHandler<GetBuyerByEmailQuery, Buyer?>
{
    private readonly IBuyerRepository _buyerRepository;

    public GetBuyerByEmailQueryHandler(IBuyerRepository buyerRepository)
    {
        _buyerRepository = buyerRepository;
    }

    public async ValueTask<Buyer?> Handle(GetBuyerByEmailQuery query, CancellationToken cancellationToken)
    {
        var buyer = await _buyerRepository.GetByEmailAsync(query.Email, cancellationToken);

        return buyer;
    }
}