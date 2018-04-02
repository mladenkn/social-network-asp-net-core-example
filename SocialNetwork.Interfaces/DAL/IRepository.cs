using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SocialNetwork.Interfaces.DAL
{
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
    {
        Task<TEntity> Update(Expression<Func<TEntity, bool>> selector, Action<TEntity> consumeItem, params string[] propsToInclude);
        TEntity Update(TEntity post);
        TEntity Insert(TEntity entity);
        Task Delete(Expression<Func<TEntity, bool>> filter);
    }
}
