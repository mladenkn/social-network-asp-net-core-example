using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork.Models
{
    public class PivotTableWrapper<TPivotTableRow, TEntity> : ICollection<TEntity>
    {
        private readonly ICollection<TPivotTableRow> _rows;
        private readonly Func<TPivotTableRow, TEntity> _getEntity;
        private readonly Func<TPivotTableRow, bool> _filter;

        public PivotTableWrapper(ICollection<TPivotTableRow> rows, Func<TPivotTableRow, TEntity> GetEntity,
            Func<TPivotTableRow, bool> filter)
        {
            _rows = rows;
            _getEntity = GetEntity;
            _filter = filter;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return
                _rows
                    .Where(_filter)
                    .Select(_getEntity).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TEntity item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; }
        public bool IsReadOnly { get; }
    }
}
