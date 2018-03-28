using System.Collections;
using System.Collections.Generic;

namespace Utilities
{
    public class ReadOnlyCollectionWrapper<T> : IReadOnlyCollection<T>
    {
        private readonly ICollection<T> _wrapee;

        public ReadOnlyCollectionWrapper(ICollection<T> wrapee)
        {
            _wrapee = wrapee;
            Count = _wrapee.Count;
        }

        public IEnumerator<T> GetEnumerator() => _wrapee.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count { get; }
    }
}
