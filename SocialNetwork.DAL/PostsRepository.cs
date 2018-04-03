using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Models;

namespace SocialNetwork.DAL
{
    public class PostsRepository : Repository<Post>, IPostsRepository
    {
        public PostsRepository(DbSet<Post> wrapeeContainer) : base(wrapeeContainer)
        {
        }

        public Task<IList<Post>> GetManyOrderedByDateDescending(Expression<Func<Post, bool>> filter = null, int? count = null, int skip = 0,
            params string[] propsToInclude)
        {
            IQueryable<Post> query =
                propsToInclude.Any()
                    ? _wrapeeContainer.Include(propsToInclude[0])
                    : _wrapeeContainer;

            query = query.OrderByDescending(it => it.CreatedAt);
            query = filter != null ? query.Where(filter) : query;
            query = query.Skip(skip);
            query = count != null ? query.Take(count.Value) : query;

            return query
                .ToListAsync()
                .Map(it => (IList<Post>)it);
        }
    }
}
