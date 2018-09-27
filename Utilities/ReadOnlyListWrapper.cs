using System.Collections;
using System.Collections.Generic;

namespace Utilities
{
    public class ReadOnlyListWrapper<T> : IReadOnlyList<T>
    {
        private readonly IList<T> _list;

        public ReadOnlyListWrapper(IList<T> list)
        {
            _list = list;
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        public int Count => _list.Count;

        public T this[int index] => _list[index];
    }
}
