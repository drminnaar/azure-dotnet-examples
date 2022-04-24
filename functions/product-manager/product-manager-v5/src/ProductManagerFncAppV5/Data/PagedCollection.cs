using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagerFncAppV5.Data
{
    interface IPagedCollection<T> : IEnumerable<T>
    {
        int CurrentPageNumber { get; }
        int? NextPageNumber { get; }
        int? PreviousPageNumber { get; }
        int LastPageNumber { get; }
        int ItemCount { get; }
        int PageSize { get; }
        int PageCount { get; }
        bool HasPrevious { get; }
        bool HasNext { get; }
    }

    [Serializable]
    internal class PagedCollection<T> : IPagedCollection<T>
    {
        [NonSerialized]
        private readonly List<T> _list = new();

        public PagedCollection(IReadOnlyList<T> items, int itemCount, int pageNumber, int pageSize)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (items.Any(item => item == null))
                throw new ArgumentException("Only non-nullable items allowed", (nameof(items)));

            if (itemCount < 0)
                throw new ArgumentOutOfRangeException(nameof(itemCount), "Value must be >= 0");

            if (pageNumber < 0)
                throw new ArgumentException("Value must be >= 0", nameof(pageNumber));

            if (pageSize < 0)
                throw new ArgumentException("Value must be >= 0", nameof(pageSize));

            ItemCount = itemCount;
            CurrentPageNumber = pageNumber;
            PageSize = pageSize;
            PageCount = ComputePageCount(pageSize, itemCount);
            _list.AddRange(items);
        }

        private PagedCollection()
        {
            ItemCount = 0;
            CurrentPageNumber = 0;
            PageSize = 0;
            PageCount = 0;
        }

        public int CurrentPageNumber { get; }
        public int ItemCount { get; }
        public int PageSize { get; }
        public int PageCount { get; }
        public int LastPageNumber => PageCount;
        public int? NextPageNumber => HasNext ? CurrentPageNumber + 1 : default(int?);
        public int? PreviousPageNumber => HasPrevious ? CurrentPageNumber - 1 : default(int?);
        public bool HasPrevious => CurrentPageNumber > 1;
        public bool HasNext => CurrentPageNumber < PageCount;

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        private static int ComputePageCount(int pageSize, int itemCount) =>
            pageSize > 0 ? (int)Math.Ceiling(itemCount / (double)pageSize) : 0;

        public static IPagedCollection<T> Empty => new PagedCollection<T>();
    }
}
