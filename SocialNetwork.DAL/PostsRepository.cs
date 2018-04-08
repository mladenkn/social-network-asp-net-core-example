using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Interface.DAL;
using SocialNetwork.Models;

namespace SocialNetwork.DAL
{
    public class PostsRepository : Repository<Post>, IPostsRepository
    {
        public PostsRepository(DbSet<Post> wrapedContainer) : base(wrapedContainer)
        {
        }

        public Task<IList<Post>> GetMany(Expression<Func<Post, bool>> filter = null, int? count = null, int skip = 0,
                                         PostsOrder? order = null, params string[] propsToInclude)
        {
            IQueryable<Post> query =
                propsToInclude.Any()
                    ? _wrapedContainer.Include(propsToInclude[0])
                    : _wrapedContainer;

            if(order == PostsOrder.CreatedAtDescending)
                query = query.OrderByDescending(it => it.CreatedAt);

            if (filter != null)
                query = query.Where(filter);

            query = query.Skip(skip);

            if (count != null)
                query = query.Take(count.Value);

            return query
                .ToListAsync()
                .Map(it => (IList<Post>)it);
        }
    }
}
