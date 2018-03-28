using System.Threading.Tasks;
using SocialNetwork.Interfaces.DAL;

namespace SocialNetwork.DAL
{
    public class DatabaseOperations : IDatabaseOperations
    {
        private readonly SocialNetworkDbContext _dbContext;

        public DatabaseOperations(SocialNetworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
    }
}
