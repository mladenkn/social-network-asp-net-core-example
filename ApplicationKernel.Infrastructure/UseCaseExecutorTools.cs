using ApplicationKernel.Domain;
using ApplicationKernel.Domain.DataPersistance;
using ApplicationKernel.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ApplicationKernel.Infrastructure
{
    public class UseCaseExecutorTools : IUseCaseExecutorTools
    {
        private readonly DbContext _db;

        public UseCaseExecutorTools(DbContext db)
        {
            _db = db;
        }

        public IQueryable<T> Query<T>()
            where T : class
        {
            return _db.Set<T>();
        }

        public IDatabaseTransaction Transaction() => new EfDatabaseTransaction(_db);
    }
}
