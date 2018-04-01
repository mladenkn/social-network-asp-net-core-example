using System;
using System.Linq;
using System.Text;

namespace Utilities
{
    public static class Utils
    {
        public static Random Random { get; } = new Random();

        public static TResult Let<T, TResult>(this T o, Func<T, TResult> func) => func(o);

        public static U LetIf<T, U>(this T o, Func<T, bool> predicate, Func<T, U> func)
        {
            if (predicate(o))
                return func(o);
            else
            {
                if (o is U u)
                    return u;
                else
                    throw new Exception();
            }
        }

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

        public static void AssertIsTrue(bool boolean, Exception e)
        {
            if (!boolean)
                throw e;
        }

        public static void AssertIsTrue(bool boolean) => AssertIsTrue(boolean, new Exception("Assertation failed"));

        public static U CastIt<T, U>(T o) where U : T => (U) o;

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
            Enumerable.Repeat(toAppend, count)
                .ForEach(stringBuilder.Append);
            return stringBuilder;
        }

        public static StringBuilder RemoveLast(this StringBuilder stringBuilder)
        {
            stringBuilder.Length--;
            return stringBuilder;
        }

        public static bool DoesUserNameSatisfy(string userName)
        {
            return userName == "mladen";
        }
    }
}
