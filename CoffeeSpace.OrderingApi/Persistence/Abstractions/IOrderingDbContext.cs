﻿using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.Orders;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.OrderingApi.Persistence.Abstractions;

public interface IOrderingDbContext
{
    DbSet<Order> Orders { get; init; }

    DbSet<OrderItem> OrderItems { get; init; }

    DbSet<Buyer> Buyers { get; init; }

    DbSet<Address> Addresses { get; init; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}