using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SocialNetwork.Interface.Models.Entities;

namespace SocialNetwork.Interface.DAL
{
    public interface IPostsRepository : IRepository<Post>
    {
        Task<IList<Post>> GetMany(Expression<Func<Post, bool>> filter = null,
            int? count = null, int skip = 0, PostsOrder? order = null, params string[] propsToInclude);
    }

    public enum PostsOrder
    {
        CreatedAtDescending
    }
}
