using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.ViewModels;
using Mediator;

namespace CoffeeSpace.AClient.Messages.Commands.Handlers;

public sealed class CreateOrderHistoryCommandHandler : ICommandHandler<CreateOrderHistoryCommand>
{
    private readonly OrderHistoryWindowViewModel _historyWindowViewModel;

    public CreateOrderHistoryCommandHandler(OrderHistoryWindowViewModel historyWindowViewModel)
    {
        _historyWindowViewModel = historyWindowViewModel;
    }

    public ValueTask<Unit> Handle(CreateOrderHistoryCommand command, CancellationToken cancellationToken)
    {
        bool isContains = _historyWindowViewModel.Orders.Contains(command.Order);
        if (!isContains)
        {
            _historyWindowViewModel.Orders.Add(command.Order);
        }
        
        return Unit.ValueTask;
    }
}