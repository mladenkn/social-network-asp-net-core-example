using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SocialNetwork.Interfaces.DAL
{
    public interface IRepository<TEntity>
    {
        Task<IList<TEntity>> GetMany(Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>> orderBy = null,
            int? count = null, int skip = 0, params string[] propsToInclude);

        Task<TEntity> GetOne(Expression<Func<TEntity, bool>> selector = null, params string[] propsToInclude);

        Task<TEntity> Update(Expression<Func<TEntity, bool>> selector, Action<TEntity> consumeItem, params string[] propsToInclude);
        TEntity Update(TEntity post);
        TEntity Insert(TEntity entity);
        Task Delete(Expression<Func<TEntity, bool>> filter);
    }
}
