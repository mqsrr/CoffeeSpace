using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Register;
using Mediator;

namespace CoffeeSpace.IdentityApi.Application.Messages.Commands;

public sealed class RegisterUserCommand : ICommand<string?>
{
    public required RegisterRequest Request { get; init; }
}