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
    }
}
