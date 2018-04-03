using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Interfaces.DAL;

namespace SocialNetwork.DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbSet<TEntity> _wrapeeContainer;

        public Repository(DbSet<TEntity> wrapeeContainer)
        {
            _wrapeeContainer = wrapeeContainer;
        }

        public Task<IList<TEntity>> GetMany(Expression<Func<TEntity, bool>> filter = null,
                                            int? count = null, int skip = 0, params string[] propsToInclude)
        {
            IQueryable<TEntity> query =
                propsToInclude.Any() 
                    ? _wrapeeContainer.Include(propsToInclude[0])
                    : _wrapeeContainer;

            if (filter != null)
                query = query.Where(filter);

            query = query.Skip(skip);

            if (count != null)
                query = query.Take(count.Value);

            return query
                .ToListAsync()
                .Map(it => (IList<TEntity>)it);
        }

        public Task<IList<TProperty>> GetMany<TProperty>(Expression<Func<TEntity, TProperty>> selector, 
                                                       Expression<Func<TEntity, bool>> filter = null,
                                                       int? count = null, int skip = 0)
        {
            IQueryable<TEntity> query_ = _wrapeeContainer;

            if(filter != null)
                query_ = query_.Where(filter);

            IQueryable<TProperty> query = query_.Select(selector);

            query = query.Skip(skip);

            if (count != null)
                query = query.Take(count.Value);

            return query
                .ToListAsync()
                .Map(it => (IList<TProperty>)it);
        }

        public Task<TEntity> GetOne(Expression<Func<TEntity, bool>> selector = null, params string[] propsToInclude)
        {
            IQueryable<TEntity> query = 
                propsToInclude.Any()
                    ? _wrapeeContainer.Include(propsToInclude[0])
                    : _wrapeeContainer;

            if (selector != null)
                return query.SingleAsync(selector);
            else
                return query.SingleAsync();
        }

        public async Task<TEntity> Update(Expression<Func<TEntity, bool>> selector, Action<TEntity> consumeItem, params string[] propsToInclude)
        {
            var item = await GetOne(selector, propsToInclude);
            consumeItem(item);
            return _wrapeeContainer.Update(item).Entity;
        }

        public TEntity Update(TEntity item)
        {
            return _wrapeeContainer.Update(item).Entity;
        }

        public virtual TEntity Insert(TEntity entity)
        {
            return _wrapeeContainer.Add(entity).Entity;
        }

        public async Task Delete(Expression<Func<TEntity, bool>> filter)
        {
            TEntity item = await GetOne(filter);
            _wrapeeContainer.Remove(item);
        }
    }
}
