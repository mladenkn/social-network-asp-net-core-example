using ApplicationKernel.Domain.DataPersistance;
using ApplicationKernel.Domain.DataQueries;
using System.Linq;

namespace ApplicationKernel.Domain
{
    public interface IUseCaseExecutorTools
    {
        IQuery Query { get; }
        IUnitOfWork UnitOfWork { get; }
    }
}
