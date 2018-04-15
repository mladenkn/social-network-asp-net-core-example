using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.DAL;

namespace SocialNetwork.UnitTests
{
    static class TestUtils
    {
        public static async Task<(SocialNetworkDbContext dbContext, SqliteConnection connection)> InitDbInMemory()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<SocialNetworkDbContext>()
                .UseSqlite(connection)
                .Options;

            var dbContext = new SocialNetworkDbContext(options);

            await dbContext.Database.EnsureCreatedAsync();

            return (dbContext, connection);
        }

        public static async Task<SocialNetworkDbContext> ConnectToPersistentDatabase()
        {
            var options = new DbContextOptionsBuilder<SocialNetworkDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;" +
                              "Database=SocialNetwork;" +
                              "Trusted_Connection=True;" +
                              "MultipleActiveResultSets=true")
                .Options;

            var dbContext = new SocialNetworkDbContext(options);

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            return dbContext;
        }
    }
}
