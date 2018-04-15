using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public class CollectionWrapper<T, U> : EnumerableWrapper<T, U, ICollection<T>>, ICollection<U>
    {
        private readonly ICollection<T> _wrapee;
        private readonly Func<U, T> _reverseMapper;
        private readonly Func<T, bool> _filter;

        public CollectionWrapper(ICollection<T> wrapee, Func<T, U> mapper, Func<U, T> reverseMapper, Func<T, bool> filter = null)
            : base(wrapee, mapper, filter)
        {
            _wrapee = wrapee;
            _reverseMapper = reverseMapper;
            _filter = filter;
        }

        public void Add(U item)
        {
            T mapped = _reverseMapper(item);
            _wrapee.Add(mapped);
        }

        public void Clear() => _wrapee.Clear();

        public bool Contains(U item)
        {
            T mapped = _reverseMapper(item);
            return _wrapee.Contains(mapped);
        }

        public void CopyTo(U[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(U item)
        {
            T mapped = _reverseMapper(item);
            return _wrapee.Remove(mapped);
        }

        public int Count => this.Count();
        public bool IsReadOnly => _wrapee.IsReadOnly;
    }
}
