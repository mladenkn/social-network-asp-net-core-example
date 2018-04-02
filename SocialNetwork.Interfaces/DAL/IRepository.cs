using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SocialNetwork.Interfaces.DAL
{
    public interface IRepository<TEnity>
    {
        Task<IList<TEnity>> GetMany(Expression<Func<TEnity, bool>> filter = null,
            Expression<Func<IQueryable<TEnity>, IOrderedQueryable<TEnity>>> orderBy = null,
            int? count = null, int skip = 0, params string[] propsToInclude);

        Task<TEnity> GetOne(Expression<Func<TEnity, bool>> selector = null, params string[] propsToInclude);

        Task<TEnity> Update(Expression<Func<TEnity, bool>> selector, Action<TEnity> consumeItem, params string[] propsToInclude);
        TEnity Update(TEnity post);
        TEnity Insert(TEnity entity);
        Task Delete(Expression<Func<TEnity, bool>> filter);
    }
}
