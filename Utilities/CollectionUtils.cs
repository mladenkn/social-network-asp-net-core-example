using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class CollectionUtils
    {
        private static readonly Random _random = new Random();

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var o in enumerable)
                action(o);
        }

        public static void ForEach<T, U>(this IEnumerable<T> enumerable, Func<T, U> action)
        {
            foreach (var o in enumerable)
                action(o);
        }

        public static IReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> collection) => new ReadOnlyCollectionWrapper<T>(collection);

        public static IReadOnlyList<T> AsReadOnly<T>(this IList<T> collection) => new ReadOnlyListWrapper<T>(collection);

        public static bool ContainsAll<T>(this IEnumerable<T> enumerable, IEnumerable<T> items)
        {
            return items.All(enumerable.Contains);
        }

        public static void RemoveIf<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            foreach (var item in collection.ToArray())
            {
                if (predicate(item))
                    collection.Remove(item);
            }
        }

        public static TAcumulator Aggregate<TElement, TAcumulator>(this IEnumerable<TElement> source, TAcumulator acumulator,
            Action<TAcumulator, TElement> action)
        {
            var result = source.Aggregate(acumulator, (accumulator, element) =>
            {
                action(acumulator, element);
                return acumulator;
            });
            return result;
        }
    }
}

