using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Buyers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Queries.Buyers;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Services;

internal sealed class BuyerService : IBuyerService
{
    private readonly ISender _sender;

    public BuyerService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Buyer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var buyer = await _sender.Send(new GetBuyerByIdQuery
        {
            Id = id.ToString()
        }, cancellationToken);

        return buyer;
    }

    public async Task<Buyer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var buyer = await _sender.Send(new GetBuyerByEmailQuery
        {
            Email = email
        }, cancellationToken);

        return buyer;
    }

    public async Task<bool> CreateAsync(Buyer buyer, CancellationToken cancellationToken)
    {
        var created = await _sender.Send(new CreateBuyerCommand
        {
            Buyer = buyer
        }, cancellationToken);

        return created;
    }

    public async Task<Buyer?> UpdateAsync(Buyer buyer, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new UpdateBuyerCommand
        {
            Buyer = buyer
        }, cancellationToken);
        
        return result;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _sender.Send(new DeleteBuyerByIdCommand
        {
            Id = id.ToString()
        }, cancellationToken);

        return deleted;
    }
}