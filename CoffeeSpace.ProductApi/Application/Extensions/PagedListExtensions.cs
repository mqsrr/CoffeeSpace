using CoffeeSpace.ProductApi.Application.Helpers;

namespace CoffeeSpace.ProductApi.Application.Extensions;

public static class PagedListExtensions
{
    public static PagedList<TEntity> ToPagedList<TEntity>(this IEnumerable<TEntity> entities, int page, int pageSize, int count)
    {
        return new PagedList<TEntity>
        {
            Items = entities,
            Page = page,
            PageSize = pageSize,
            Count = count
        };
    }
}