using System.Threading.Tasks;
using ApplicationKernel.Domain;
using ApplicationKernel.Domain.DataPersistance;
using Microsoft.EntityFrameworkCore;

namespace ApplicationKernel.Infrastructure.Database
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public EfUnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(IEntity entity) => _dbContext.Add(entity);

        public void Update(IEntity entity) => _dbContext.Update(entity);

        public void Delete(IEntity entity) => _dbContext.Remove(entity);

        public void Delete<T>(T entity) where T : IDeletable, IEntity
        {
            entity.IsDeleted = true;
            Update(entity);
        }

        public async Task PersistChanges() => await _dbContext.SaveChangesAsync();
    }
}
