using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SocialNetwork.DAL;
using SocialNetwork.DevelopmentUtilities;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Models;
using SocialNetwork.Web.Utilities;
using Utilities;
using Xunit;

namespace SocialNetwork.UnitTests
{
    public class UsersRepositoryTest
    {
        private readonly SqliteConnection _dbConnection;
        private readonly SocialNetworkDbContext _dbContext;
        private readonly IRepository<User> _usersRepo;
        private readonly IRepository<Post> _postsRepo;
        private readonly TestDataContainer _testDataContainer;

        public UsersRepositoryTest()
        {
            // arange
            var (dbContext, connection) = TestUtils.InitDbInMemory().Result;
            _dbConnection = connection;
            _dbContext = dbContext;
            _usersRepo = new Repository<User>(_dbContext.Users);
            _postsRepo = new Repository<Post>(_dbContext.Posts);

            _testDataContainer = new TestDataContainer();
        }

        private async Task SaveData(IEnumerable<User> users, IEnumerable<Post> posts)
        {
            users.ForEach(_usersRepo.Insert);
            await _dbContext.SaveChangesAsync();
            posts.ForEach(_postsRepo.Insert);
            await _dbContext.SaveChangesAsync();
        }

        private void Cleenup()
        {
            _dbConnection?.Close();
        }

        [Fact]
        public async Task ReadUsersPost()
        {
            // arange
            var usersToSave = _testDataContainer.Users.Values;
            var postsToSave = _testDataContainer.Posts;

            // act
            await SaveData(usersToSave, postsToSave);

            // prepare assert
            IList<User> users = await _usersRepo.GetMany();
            users.ForEach(it => it.Posts);

            Cleenup();
        }

        [Fact]
        public async Task ReadById()
        {
            // arange
            var usersToSave = _testDataContainer.Users.Values.ToList();
            var postsToSave = _testDataContainer.Posts;
            const string userName = "mate";
            Generator.RandomUser(userName, userName).Also(usersToSave.Add);

            // act
            await SaveData(usersToSave, postsToSave);

            // prepare assert
            User user = await _usersRepo.GetOne(userName);

            // assert
            Assert.True(user.Id == userName);

            Cleenup();
        }
    }
}
