using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationKernel.Domain
{
    public class PagedList<T>
    {
        internal PagedList(IReadOnlyList<T> items, int currentPageNumber, int totalCount)
        {
            Items = items;
            CurrentPageNumber = currentPageNumber;
            TotalCount = totalCount;
        }

        public IReadOnlyList<T> Items { get; }
        public int CurrentPageNumber { get; }
        public int TotalCount { get; }
    }

    public static class PagedList
    {
        public static PagedList<T> Create<T>(IReadOnlyList<T> items, int currentPage, int totalCount)
        {
            return new PagedList<T>(items, currentPage, totalCount);
        }
    }
}
