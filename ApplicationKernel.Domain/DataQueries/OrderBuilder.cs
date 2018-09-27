using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ApplicationKernel.Domain.DataQueries
{
    public class OrderBuilder<TObject>
    {
        private readonly List<OrderCriteria<TObject>> _list = new List<OrderCriteria<TObject>>();

        public OrderBuilder<TObject> Ascending(Expression<Func<TObject, object>> property)
        {
            _list.Add(new OrderCriteria<TObject>(property, OrderType.Ascending));
            return this;
        }

        public OrderBuilder<TObject> Descending(Expression<Func<TObject, object>> property)
        {
            _list.Add(new OrderCriteria<TObject>(property, OrderType.Descending));
            return this;
        }

        public IReadOnlyList<OrderCriteria<TObject>> Build() => _list;
    }

    public static class OrderBuilder
    {
        public static OrderBuilder<TObject> Of<TObject>() => new OrderBuilder<TObject>();
    }
}
