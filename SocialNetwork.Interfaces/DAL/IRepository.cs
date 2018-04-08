using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SocialNetwork.Interface.DAL
{
    public interface IRepository<TEntity>
    {
        Task<IList<TEntity>> GetMany(Expression<Func<TEntity, bool>> filter = null,
            int? count = null, int skip = 0, params string[] propsToInclude);

        Task<IList<TProperty>> GetMany<TProperty>(Expression<Func<TEntity, TProperty>> selector,
            Expression<Func<TEntity, bool>> filter = null,
            int? count = null, int skip = 0);

        Task<TEntity> GetOne(Expression<Func<TEntity, bool>> selector = null, params string[] propsToInclude);

        Task<TEntity> Update(Expression<Func<TEntity, bool>> selector, Action<TEntity> consumeItem, params string[] propsToInclude);
        TEntity Update(TEntity post);
        TEntity Insert(TEntity entity);
        Task Delete(Expression<Func<TEntity, bool>> filter);

        void Delete(TEntity entity);
    }
}
