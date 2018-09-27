using System;
using System.Data.Common;
using SocialNetwork.Infrastructure;

namespace SocialNetwork.Tests.Database
{
    public class DatabaseTest : IDisposable
    {
        protected readonly SocialNetworkDbContext _db;
        protected readonly DbConnection _dbConnection;

        public DatabaseTest()
        {
            (_db, _dbConnection) = TestFactory.ConnectToDatabase().Result;
        }

        public void Dispose()
        {
            _db.Dispose();
            _dbConnection?.Dispose();
        }
    }
}
