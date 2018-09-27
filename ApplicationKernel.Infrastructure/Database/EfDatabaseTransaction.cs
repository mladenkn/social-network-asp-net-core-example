using System.Threading.Tasks;
using ApplicationKernel.Domain;
using ApplicationKernel.Domain.DataPersistance;
using Microsoft.EntityFrameworkCore;

namespace ApplicationKernel.Infrastructure.Database
{
    public class EfDatabaseTransaction : IDatabaseTransaction
    {
        private readonly DbContext _dbContext;

        public EfDatabaseTransaction(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IDatabaseTransaction Save(IEntity entity)
        {
            _dbContext.Add(entity);
            return this;
        }

        public IDatabaseTransaction Update(IEntity entity)
        {
            _dbContext.Update(entity);
            return this;
        }

        public IDatabaseTransaction Delete(IEntity entity)
        {
            _dbContext.Remove(entity);
            return this;
        }

        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}
