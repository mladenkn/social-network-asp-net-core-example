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

        public static T[] IntoArray<T>(this T o) => new[] {o};

        public static void Assert(bool boolean, Exception e)
        {
            if (!boolean)
                throw e;
        }

        public static void Assert(bool boolean) => Assert(boolean, new Exception("Assertation failed"));

        public static U UpCast<T, U>(this T o) where U : T => (U)o;
        public static U DownCast<T, U>(this T o) where T : U => o;
        public static T AnyCast<T>(this object o) => (T) o;

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
            foreach (var i in Enumerable.Range(0, times))
                action();
        }

        public static T PickOne<T>(this Random random, params T[] args)
        {
            var index = random.Next(args.Length);
            return args[index];
        }

        public static bool NextBool(this Random random) => random.Next(0, 1) == 1;

        public static bool EqualsAny<T>(this T o, params T[] objects) => objects.Any(it => o.Equals(it));
    }
}
