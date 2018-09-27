using System;
using System.Linq.Expressions;

namespace ApplicationKernel.Domain.DataQueries
{
    public struct OrderCriteria<TEntity>
    {
        public Expression<Func<TEntity, object>> Property { get; }
        public OrderType OrderType { get; }

        public OrderCriteria(Expression<Func<TEntity, object>> property, OrderType orderType)
        {
            Property = property;
            OrderType = orderType;
        }
    }

    public enum OrderType { Ascending, Descending }
}
