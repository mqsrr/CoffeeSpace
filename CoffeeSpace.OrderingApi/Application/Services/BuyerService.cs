using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Messages.Buyers;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Services;

internal sealed class BuyerService : IBuyerService
{
    private readonly ITopicProducer<UpdateBuyer> _topicProducer;
    private readonly IBuyerRepository _buyerRepository;

    public BuyerService(IBuyerRepository buyerRepository, ITopicProducer<UpdateBuyer> topicProducer)
    {
        _buyerRepository = buyerRepository;
        _topicProducer = topicProducer;
    }

    public Task<Buyer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var buyer = _buyerRepository.GetByIdAsync(id, cancellationToken);
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

        await _topicProducer.Produce(new
        {
            Buyer = updatedBuyer
        }, cancellationToken).ConfigureAwait(false);

        return updatedBuyer;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var buyerToDelete = await _buyerRepository.GetByIdAsync(id, cancellationToken);
        if (buyerToDelete is null)
        {
            return false;
        }

        bool isDeleted = await _buyerRepository.DeleteByIdAsync(id, cancellationToken);
        if (!isDeleted)
        {
            return false;
        }

        await _topicProducer.Produce(new
        {
            buyerToDelete.Name,
            buyerToDelete.Email
        }, cancellationToken).ConfigureAwait(false);

        return isDeleted;
    }
}