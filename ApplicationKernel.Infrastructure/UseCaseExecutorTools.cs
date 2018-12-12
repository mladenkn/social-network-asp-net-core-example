using ApplicationKernel.Domain;
using ApplicationKernel.Domain.DataPersistance;
using ApplicationKernel.Domain.DataQueries;
using ApplicationKernel.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ApplicationKernel.Infrastructure
{
    public class UseCaseExecutorTools : IUseCaseExecutorTools
    {
        public UseCaseExecutorTools(DbContext db, IQuery query)
        {
            Query = query;
            UnitOfWork = new EfUnitOfWork(db);
        }

        public IQuery Query { get; }
        public IUnitOfWork UnitOfWork { get; }
    }
}
