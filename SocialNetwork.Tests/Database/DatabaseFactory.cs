using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Infrastructure;

namespace SocialNetwork.Tests.Database
{
    public static class TestFactory
    {
        public static async Task<(SocialNetworkDbContext Database, DbConnection DatabaseConnection)> ConnectToDatabase()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<SocialNetworkDbContext>()
                .UseSqlite(connection)
                .Options;
            
            var db = new SocialNetworkDbContext(options);
            await db.Database.EnsureCreatedAsync();

            return (db, connection);
        }

        //public static async Task<(SocialNetworkDbContext Database, DbConnection DatabaseConnection)> ConnectToDatabase2()
        //{
        //    var connectionString = @"Server=.;
        //                        Database=SocialNetwork;
        //                        Trusted_Connection=True;
        //                        ConnectRetryCount=0";

        //    var options = new DbContextOptionsBuilder<SocialNetworkDbContext>()
        //        .UseSqlServer(connectionString)
        //        .Options;

        //    var db = new SocialNetworkDbContext(options);
        //    await db.Database.EnsureCreatedAsync();

        //    return (db, null);
        //}
    }
}
