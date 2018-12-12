using System;
using System.Linq;
using Utilities;

namespace ApplicationKernel.Domain.DataQueries
{
    public interface IQuery
    {
        IQueryable<T> Of<T>() where T : class;
    }
}
