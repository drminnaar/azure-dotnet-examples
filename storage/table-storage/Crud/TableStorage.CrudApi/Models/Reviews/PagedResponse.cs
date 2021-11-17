using System.Collections.Generic;

namespace TableStorage.CrudApi.Models;

public sealed record PagedResponse<T>
{
    public PagedResponse(IPagedCollection<T> items)
    {
        CurrentPageNumber = items.CurrentPageNumber;
        HasNext = items.HasNext;
        HasPrevious = items.HasPrevious;
        ItemCount = items.ItemCount;
        Items = items;
        LastPageNumber = items.LastPageNumber;
        NextPageNumber = items.NextPageNumber;
        PageCount = items.PageCount;
        PageSize = items.PageSize;
        PreviousPageNumber = items.PreviousPageNumber;
    }

    public int CurrentPageNumber { get; init; }
    public int ItemCount { get; init; }
    public int PageSize { get; init; }
    public int PageCount { get; init; }
    public int LastPageNumber { get; init; }
    public int? NextPageNumber { get; init; }
    public int? PreviousPageNumber { get; init; }
    public bool HasPrevious { get; init; }
    public bool HasNext { get; init; }
    public IEnumerable<T> Items { get; init; }

}
