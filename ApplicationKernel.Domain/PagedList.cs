using System.Collections.Generic;

namespace ApplicationKernel.Domain
{
    public struct PagedList<T>
    {
        internal PagedList(IReadOnlyList<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }

        public IReadOnlyList<T> Items { get; }
        public int TotalCount { get; }
    }

    public static class PagedList
    {
        public static PagedList<T> Of<T>(IReadOnlyList<T> items, int totalCount)
        {
            return new PagedList<T>(items, totalCount);
        }
    }
}
