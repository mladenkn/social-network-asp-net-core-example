using System.Threading.Tasks;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Models;
using SocialNetwork.TestingUtilities;
using Utilities;

namespace SocialNetwork.Services
{
    public class DbSeeder
    {
        private readonly IRepository<Post> _postsRepository;
        private readonly IRepository<User> _usersRepository;
        private readonly TestDataContainer _data;
        private readonly IDatabaseOperations _dbOps;

        public DbSeeder(IRepository<Post> postsRepository, IRepository<User> usersRepository, TestDataContainer data,
                        IDatabaseOperations dbOps)
        {
            _postsRepository = postsRepository;
            _usersRepository = usersRepository;
            _data = data;
            _dbOps = dbOps;
        }

        public Task Seed()
        {
            _data.Posts.ForEach(_postsRepository.Insert);
            _data.Users.Values.ForEach(_usersRepository.Insert);

            return _dbOps.SaveChangesAsync();
        }
    }
}
