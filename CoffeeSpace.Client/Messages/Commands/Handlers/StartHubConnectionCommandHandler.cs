using CoffeeSpace.Client.Services.Abstractions;
using Mediator;

namespace CoffeeSpace.Client.Messages.Commands.Handlers;

public sealed class StartHubConnectionCommandHandler : ICommandHandler<StartHubConnectionCommand, bool>
{
    private readonly IHubConnectionService _hubConnectionService;

    public StartHubConnectionCommandHandler(IHubConnectionService hubConnectionService)
    {
        _hubConnectionService = hubConnectionService;
    }

    public async ValueTask<bool> Handle(StartHubConnectionCommand command, CancellationToken cancellationToken)
    {
        bool isConnected = _hubConnectionService.IsConnected;
        if (isConnected)
        {
            return true;
        }

        string buyerId = await SecureStorage.GetAsync("buyer-id");
        await _hubConnectionService.StartConnectionAsync(buyerId, cancellationToken);

        return true;
    }
}
