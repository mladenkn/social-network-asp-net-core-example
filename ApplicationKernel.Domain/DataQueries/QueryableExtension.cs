using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ApplicationKernel.Domain.DataQueries
{
    public static class QueryableExtension
    {
        public static IQueryable<T> Order<T>(this IQueryable<T> queryable, IEnumerable<OrderCriteria<T>> order)
        {
            foreach (var orderCriteria in order)
            {
                if (orderCriteria.OrderType == OrderType.Ascending)
                    queryable = queryable.OrderBy(orderCriteria.Property);
                else if (orderCriteria.OrderType == OrderType.Descending)
                    queryable = queryable.OrderByDescending(orderCriteria.Property);
                else
                    throw new InvalidEnumArgumentException();
            }
            return queryable;
        }

        public static IQueryable<T> Paging<T>(this IQueryable<T> queryable, Paging paging)
        {
            var skipCount = (paging.PageNumber - 1) * paging.PageSize;
            var takeCount = paging.PageSize;
            return queryable.Skip(skipCount).Take(takeCount);
        }
    }
}
