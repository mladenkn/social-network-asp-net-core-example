using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Utilities
{
    public static class Utils
    {
        public static IQueryable<TEntity> IncludeMany<TEntity>(this IQueryable<TEntity> source, IEnumerable<string> toInclude) where TEntity : class
        {
            foreach (var prop in toInclude)
                source = source.Include(prop);
            return source;
        }

        public static IQueryable<TEntity> IncludeMany<TEntity>(this IQueryable<TEntity> source, params string[] toInclude) where TEntity : class
        {
            return IncludeMany(source, (IEnumerable<string>) toInclude);
        }
    }
}
