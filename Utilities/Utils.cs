using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Utilities
{
    public static class Utils
    {
        public static TResult Map<T, TResult>(this T o, Func<T, TResult> func) => func(o);

        public static T Also<T>(this T o, Action<T> action)
        {
            action(o);
            return o;
        }

        public static T Also<T, U>(this T o, Func<T, U> action)
        {
            action(o);
            return o;
        }

        public static T SaveTo<T>(this T o, out T holder)
        {
            holder = o;
            return o;
        }

        public static U CastTo<U>(this object t)
        {
            return (U) t;
        }

        public static string GetPath<TEntity>(this Expression<Func<TEntity, object>> propertyProvider)
        {
            var bodyString = propertyProvider.Body.ToString();
            var indexOfPropertyName = bodyString.IndexOf('.') + 1;
            var propertyName = bodyString.Substring(indexOfPropertyName);
            return propertyName;
        }

        public static string Capitalize(this string str)
        {
            var firstChar = str[0].ToString().ToUpper();
            return new StringBuilder(str)
                .Remove(0, 1)
                .Insert(0, firstChar)
                .ToString();
        }

        public static StringBuilder AppendMany(this StringBuilder stringBuilder, string toAppend, int count)
        {
            Enumerable
                .Repeat(toAppend, count)
                .ForEach(stringBuilder.Append);
            return stringBuilder;
        }

        public static StringBuilder RemoveLast(this StringBuilder stringBuilder)
        {
            stringBuilder.Length--;
            return stringBuilder;
        }

        public static void Loop(int times, Action action)
        {
            foreach (var _ in Enumerable.Range(0, times))
                action();
        }

        public static bool EqualsAny<T>(this T o, params T[] objects) => objects.Any(it => o.Equals(it));

        public static bool EqualsAll<T>(this T o, params T[] objects) => objects.All(it => o.Equals(it));

        public static T GetPropValue<T>(object src, string propName)
        {
            return (T) src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public static Func<T, U> ToFunc<T, U>(this IReadOnlyDictionary<T, U> dict)
        {
            U Func(T t) => dict[t];
            return Func;
        }
    }
}
