using ApplicationKernel.Domain.DataPersistance;
using System.Linq;

namespace ApplicationKernel.Domain
{
    public interface IUseCaseExecutorTools
    {
        IQueryable<T> Query2<T>() where T : class;
        IQueryable<T> Query<T>() where T : class, IDeletable;
        IDatabaseTransaction Transaction();
    }
}
