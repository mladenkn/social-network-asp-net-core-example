using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Utilities
{
    public static class Utils
    {
        public static T SaveTo<T>(this T o, out T holder)
        {
            holder = o;
            return o;
        }

        public static string Capitalize(this string str)
        {
            var firstChar = str[0].ToString().ToUpper();
            return new StringBuilder(str)
                .Remove(0, 1)
                .Insert(0, firstChar)
                .ToString();
        }

        public static bool EqualsAny<T>(this T o, params T[] objects) => objects.Any(it => o.Equals(it));

        public static bool EqualsAll<T>(this T o, params T[] objects) => objects.All(it => o.Equals(it));

        public static Func<T, U> ToFunc<T, U>(this IReadOnlyDictionary<T, U> dict)
        {
            U Func(T t) => dict[t];
            return Func;
        }

        public static bool Implements<T>(this Type type)
        {
            return type.GetInterfaces().ContainsOne(i => i.AssemblyQualifiedName == typeof(T).AssemblyQualifiedName);
        }
    }
}
