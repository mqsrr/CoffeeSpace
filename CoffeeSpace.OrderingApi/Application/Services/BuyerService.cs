using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Messages.Buyers;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Services;

internal sealed class BuyerService : IBuyerService
{
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IBuyerRepository _buyerRepository;

    public BuyerService(IBuyerRepository buyerRepository, ISendEndpointProvider sendEndpointProvider)
    {
        _buyerRepository = buyerRepository;
        _sendEndpointProvider = sendEndpointProvider;
    }

    public Task<Buyer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var buyer = _buyerRepository.GetByIdAsync(id.ToString(), cancellationToken);
        return buyer;
    }

    public Task<Buyer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var buyer = _buyerRepository.GetByEmailAsync(email, cancellationToken);
        return buyer;
    }

    public async Task<bool> CreateAsync(Buyer buyer, CancellationToken cancellationToken)
    {
        bool isCreated = await _buyerRepository.CreateAsync(buyer, cancellationToken);
        return isCreated;
    }

    public async Task<Buyer?> UpdateAsync(Buyer buyer, CancellationToken cancellationToken)
    {
        var updatedBuyer = await _buyerRepository.UpdateAsync(buyer, cancellationToken);
        if (updatedBuyer is null)
        {
            return null;
        }

        var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:update-buyer"));
        await sendEndpoint.Send<UpdateBuyer>(new
        {
            Buyer = updatedBuyer
        }, cancellationToken).ConfigureAwait(false);

        return updatedBuyer;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var buyerToDelete = await _buyerRepository.GetByIdAsync(id.ToString(), cancellationToken);
        if (buyerToDelete is null)
        {
            return false;
        }

        bool isDeleted = await _buyerRepository.DeleteByIdAsync(id.ToString(), cancellationToken);
        if (!isDeleted)
        {
            return false;
        }

        var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:delete-buyer"));
        await sendEndpoint.Send<DeleteBuyer>(new
        {
            buyerToDelete.Name,
            buyerToDelete.Email
        }, cancellationToken).ConfigureAwait(false);

        return isDeleted;
    }
}