using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class IEnumerableWrapper<T, U> : IEnumerable<U>
    {
        private readonly IEnumerable<T> _wrapee;
        private readonly Func<T, bool> _filter;
        private readonly Func<T, U> _mapper;

        public IEnumerableWrapper(IEnumerable<T> wrapee, Func<T, U> mapper, Func<T, bool> filter = null)
        {
            _wrapee = wrapee;
            _filter = filter ?? (it => true);
            _mapper = mapper;
        }

        public IEnumerator<U> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public class Enumerator : IEnumerator<U>
        {
            private readonly IEnumerator<T> _enumerator;
            private readonly Func<T, U> _mapper;
            private readonly Func<T, bool> _filter;

            public Enumerator(IEnumerator<T> enumerator, Func<T, U> mapper, Func<T, bool> filter)
            {
                _enumerator = enumerator;
                _mapper = mapper;
                _filter = filter;
            }

            public bool MoveNext()
            {
                while (true)
                {
                    bool moveNextResult = _enumerator.MoveNext();

                    if(!moveNextResult)
                        return false;

                    if (_filter(_enumerator.Current))
                    {
                        Current = _mapper(_enumerator.Current);
                        return true;
                    }
                }
            }

            public U Current { get; private set; }

            public void Reset() => _enumerator.Reset();

            public void Dispose() => _enumerator.Dispose();

            object IEnumerator.Current => Current;
        }
    }
}
