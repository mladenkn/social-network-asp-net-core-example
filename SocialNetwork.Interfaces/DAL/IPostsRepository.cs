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
        Task<IList<Post>> GetManyOrderedByDateDescending(
            Expression<Func<Post, bool>> filter = null, int? count = null, int skip = 0, params string[] propsToInclude);
    }
}
