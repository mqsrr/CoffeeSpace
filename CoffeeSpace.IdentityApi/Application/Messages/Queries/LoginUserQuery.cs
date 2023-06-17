using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Login;
using Mediator;

namespace CoffeeSpace.IdentityApi.Application.Messages.Queries;

public sealed class LoginUserQuery : IQuery<string?>
{
    public required LoginRequest Request { get; init; }
}