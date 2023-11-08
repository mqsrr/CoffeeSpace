using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Persistence;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;

namespace CoffeeSpace.OrderingApi.Tests.Fixtures;

public sealed class OrderingDbContextFixture
{
    public required OrderingDbContext DbContext { get; init; }
    public required DbSet<Order> Orders { get; init; }
    public required DbSet<Buyer> Buyers { get; init; }

    public OrderingDbContextFixture()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoNSubstituteCustomization());

        Orders = fixture.CreateMany<Order>().AsQueryable().BuildMockDbSet();
        Buyers = fixture.CreateMany<Buyer>().AsQueryable().BuildMockDbSet();

        DbContext = fixture.Create<OrderingDbContext>();
        DbContext.Orders.Returns(Orders);
        DbContext.Buyers.Returns(Buyers);
    }
}