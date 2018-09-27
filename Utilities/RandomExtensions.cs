using System;
using System.Linq;

namespace Utilities
{
    public static class RandomExtensions
    {
        public static string NextString(this Random random, int lenght)
        {
            Enumerable
                .Range(0, lenght)
                .Select(it =>
                {
                    var charAsci = random.Next(0, 128);
                    return (char) charAsci;
                })
                .SaveTo(out var chars);

            return string.Concat(chars);
        }

        public static string NextString(this Random random)
        {
            var length = random.Next(1, 20);
            return random.NextString(length);
        }

        public static T PickOne<T>(this Random random, params T[] args)
        {
            var index = random.Next(args.Length);
            return args[index];
        }

        public static bool NextBool(this Random random) => random.Next(0, 1) == 1;
    }
}
