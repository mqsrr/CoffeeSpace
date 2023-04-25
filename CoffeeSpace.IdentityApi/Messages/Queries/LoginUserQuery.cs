using CoffeeSpace.IdentityApi.Contracts.Requests.Login;
using Mediator;

namespace CoffeeSpace.IdentityApi.Messages.Queries;

public sealed class LoginUserQuery : IQuery<string?>
{
    public required LoginRequest Request { get; init; }
}