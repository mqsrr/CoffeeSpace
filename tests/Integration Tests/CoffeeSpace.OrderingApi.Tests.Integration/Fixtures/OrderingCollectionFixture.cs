using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fixtures;

[CollectionDefinition("Ordering Dependencies")]
public sealed class OrderingCollectionFixture : ICollectionFixture<OrderingApiFactory>
{
    
}