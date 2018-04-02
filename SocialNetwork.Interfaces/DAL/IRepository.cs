using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SocialNetwork.Interfaces.DAL
{
    public interface IRepository<TEnity> : IReadOnlyRepository<TEnity>
    {
        Task<TEnity> Update(Expression<Func<TEnity, bool>> selector, Action<TEnity> consumeItem, params string[] propsToInclude);
        TEnity Update(TEnity post);
        TEnity Insert(TEnity entity);
        Task Delete(Expression<Func<TEnity, bool>> filter);
    }
}
