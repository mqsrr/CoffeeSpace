namespace CoffeeSpace.ProductApi.Application.Helpers;

public sealed class PagedList<TEntity>
{
    public required IEnumerable<TEntity> Items { get; init; }

    public required int Page { get; init; }

    public required int PageSize { get; init; }

    public required int Count { get; init; }
    
    public bool HasNextPage => Page * PageSize < Count;
    
    public bool HasPreviousPage => Page > 1;
}