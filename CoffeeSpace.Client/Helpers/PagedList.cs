using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeSpace.Client.Helpers;

public sealed class PagedList<TEntity>
{
    public required IEnumerable<TEntity> Items { get; init; }

    public required int Page { get; init; }

    public required int PageSize { get; init; }

    public required int Count { get; init; }

    public bool HasNextPage => Page * PageSize < Count;

    public bool HasPreviousPage => Page > 1;
}
