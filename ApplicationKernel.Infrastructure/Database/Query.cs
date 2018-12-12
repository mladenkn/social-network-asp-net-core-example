using ApplicationKernel.Domain;
using ApplicationKernel.Domain.DataQueries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace ApplicationKernel.Infrastructure.Database
{
    public class Query : IQuery
    {
        private readonly DbContext _dbContext;

        public Query(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Of<T>() where T : class
        {
            if (typeof(T).Implements<IDeletable>())
                return _dbContext.Set<T>().Cast<IDeletable>().Where(it => !it.IsDeleted).Cast<T>();
            else
                return _dbContext.Set<T>();
        }
    }
}
