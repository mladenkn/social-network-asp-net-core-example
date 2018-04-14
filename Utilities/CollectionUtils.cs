using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class CollectionUtils
    {
        public static IEnumerable<T> NewEnumerable<T>(Func<T> supplier, int count) =>
            Enumerable
                .Range(0, count)
                .Select(i => supplier());

        public static T[] NewArray<T>(Func<T> supplier, int count) =>
            new T[count]
                .Fill(supplier);

        public static IList<T> NewList<T>(Func<T> supplier, int count) =>
            new List<T>(count)
                .AddMany(supplier, count);

        public static HashSet<T> NewHashSet<T>(Func<T> supplier, int count) =>
            new HashSet<T>(count)
                .AddMany(supplier, count);

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

        public static T RandomElement<T>(this IReadOnlyList<T> array) =>
            Utils.Random
                .Next(array.Count)
                .Let(i => array[i]);

        public static T RandomElement<T>(this IList<T> col) =>
            Utils.Random
                .Next(col.Count)
                .Let(i => col[i]);

        // Gets thr next random element that satisfyies the condition
        public static T RandomElement<T>(this IList<T> col, Func<T, bool> condition)
        {
            if(!col.Any(condition))
                throw new Exception();

            while (true)
            {
                T generated = col.RandomElement();
                if (condition(generated))
                    return generated;
            }
        }

        public static TCollection AddMany<TCollection, TElement>(this TCollection collection, Func<TElement> supplier, int count)
            where TCollection : ICollection<TElement>
        {
            Enumerable
                .Range(0, count)
                .ForEach(i => collection.Add(supplier()));
            return collection;
        }

        public static T[] Fill<T>(this T[] array, Func<T> supplier)
        {
           Enumerable
               .Range(0, array.Length)
               .ForEach(i => array[i] = supplier());
            return array;
        }

        public static IReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> collection) => new ReadOnlyCollectionWrapper<T>(collection);

        public static bool ContainsAll<T>(this IEnumerable<T> enumerable, IEnumerable<T> items)
        {
            return items.All(enumerable.Contains);
        }

        public static IEnumerable<int> DifferencesBetweenItems(this IEnumerable<int> enumerable)
        {
            return enumerable.Zip(enumerable.Skip(1), (x, y) => y - x);
        }

        public static IEnumerable<long> DifferencesBetweenItems(this IEnumerable<long> enumerable)
        {
            return enumerable.Zip(enumerable.Skip(1), (x, y) => y - x);
        }

        public static IOrderedEnumerable<T> Schuffle<T>(this IEnumerable<T> list)
        {
            return list.OrderBy(it => it, Comparer<T>.Create((_, __) => Utils.Random.Next(-1, 1)));
        }

        public static IEnumerable<T> Concatenate<T>(this IEnumerable<IEnumerable<T>> enumerables) =>
            enumerables.SelectMany(it => it);

        public static void RemoveIf<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            foreach (var item in collection.ToArray())
            {
                if (predicate(item))
                    collection.Remove(item);
            }
        }

        public static bool HasSameContentAs<T>(this IEnumerable<T> collection, IEnumerable<T> collection2)
        {
            if (collection.Any(item => !collection2.Contains(item)))
                return false;
            if (collection2.Any(item => !collection.Contains(item)))
                return false;
            return true;
        }
    }
}
