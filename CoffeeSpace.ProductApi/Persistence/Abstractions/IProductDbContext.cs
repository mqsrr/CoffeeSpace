﻿using CoffeeSpace.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.ProductApi.Persistence.Abstractions;

public interface IProductDbContext
{
    DbSet<Product> Products { get; init; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}