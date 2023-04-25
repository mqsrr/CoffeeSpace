using System.Linq.Expressions;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CoffeeSpace.OrderingApi.Application.Extensions;

internal static class DbContextExtensions
{
    public static Task LoadDataAsync<TEntity, TProperty>(this DbSet<TEntity> dbSet, TEntity entity,
        Expression<Func<TEntity, TProperty?>> expression)
        where TEntity : class where TProperty : class
    {
        return dbSet.Entry(entity)
            .Reference(expression)
            .LoadAsync();
    }

    public static Task LoadDataAsync<TEntity, TProperty>(this DbSet<TEntity> dbSet, TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> expression)
        where TEntity : class where TProperty : class
    {
        return dbSet.Entry(entity)
            .Collection(expression)
            .LoadAsync();
    }

    public static async Task LoadDataAsync<TEntity, TProperty>(this DbSet<TEntity> dbSet,
        IEnumerable<TEntity> entities,
        Expression<Func<TEntity, TProperty>> expression)
        where TEntity : class where TProperty : class
    {
        foreach (var entity in entities)
            await dbSet.LoadDataAsync(entity, expression!);
    }

    public static async Task LoadDataAsync<TEntity, TProperty>(this DbSet<TEntity> dbSet,
        IEnumerable<TEntity> entities,
        Expression<Func<TEntity, IEnumerable<TProperty>>> expression)
        where TEntity : class where TProperty : class
    {
        foreach (var entity in entities)
            await dbSet.LoadDataAsync(entity, expression);
    }
}