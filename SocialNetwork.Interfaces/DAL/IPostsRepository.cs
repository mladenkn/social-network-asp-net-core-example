using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SocialNetwork.Models;

namespace SocialNetwork.Interfaces.DAL
{
    public interface IPostsRepository : IRepository<Post>
    {
        Task<IList<Post>> GetMany(Expression<Func<Post, bool>> filter = null,
            int? count = null, int skip = 0, PostsOrder? order = null, params string[] propsToInclude);
    }

    public enum PostsOrder
    {
        CreatedAt_Descending
    }
}
