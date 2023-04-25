using CoffeeSpace.IdentityApi.Contracts.Requests.Register;
using Mediator;

namespace CoffeeSpace.IdentityApi.Messages.Commands;

public sealed class RegisterUserCommand : ICommand<string?>
{
    public required RegisterRequest Request { get; init; }
}