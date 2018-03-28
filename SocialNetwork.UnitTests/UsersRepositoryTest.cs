using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.DAL;
using SocialNetwork.DAL.Repositories;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Models;
using SocialNetwork.TestingUtilities;
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
    }
}
