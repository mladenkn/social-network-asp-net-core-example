using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class EnumHelper
    {
        public static IEnumerable<TEnumType> GetValues<TEnumType>()
            where TEnumType : struct
        {
            return Enum.GetValues(typeof(TEnumType)).OfType<TEnumType>();
        }
    }
}
