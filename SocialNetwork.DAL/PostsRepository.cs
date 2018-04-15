using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Interface.DAL;
using SocialNetwork.Interface.Models.Entities;

namespace SocialNetwork.DAL
{
    public class PostsRepository : Repository<Post>, IPostsRepository
    {
        public PostsRepository(DbSet<Post> wrapedContainer) : base(wrapedContainer, MapPropertyNames)
        {
        }

        private static IEnumerable<string> MapPropertyNames(string propertyName)
        {
            if (propertyName == nameof(Post.LikedBy) || propertyName == nameof(Post.DislikedBy))
                return new[] {nameof(Post._Ratings)};
            else
                return new []{propertyName};
        }

        public Task<IList<Post>> GetMany(Expression<Func<Post, bool>> filter = null, int? count = null, int skip = 0,
                                         PostsOrder? order = null, params string[] propsToInclude)
        {
            IQueryable<Post> query = IncludeProperties(_wrapedContainer, propsToInclude);

            if (order == PostsOrder.CreatedAtDescending)
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
