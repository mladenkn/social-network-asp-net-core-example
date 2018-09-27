using ApplicationKernel.Domain.DataPersistance;
using System.Linq;

namespace ApplicationKernel.Domain
{
    public interface IUseCaseExecutorTools
    {
        IQueryable<T> Query<T>() where T : class;
        IDatabaseTransaction Transaction();
    }
}
