using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.Contracts.Ordering;
using CoffeeSpace.Client.Models.Ordering;
using Mediator;
using System.Collections.ObjectModel;

namespace CoffeeSpace.Client.Messages.Commands.Handlers;

public sealed class ConfirmOrdersStockCommandHandler : ICommandHandler<ConfirmOrdersStockCommand>
{
    private readonly OrderInformationViewModel _orderInformationViewModel;

    public ConfirmOrdersStockCommandHandler(OrderInformationViewModel orderInformationViewModel)
    {
        _orderInformationViewModel = orderInformationViewModel;
    }

    public ValueTask<Unit> Handle(ConfirmOrdersStockCommand command, CancellationToken cancellationToken)
    {
        _orderInformationViewModel.OrderItems = command.OrderItems.ToList();
        return Unit.ValueTask;
    }
}
