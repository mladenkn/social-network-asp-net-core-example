using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.DAL;
using SocialNetwork.DevelopmentUtilities;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Models;
using SocialNetwork.Web.Utilities;
using Utilities;
using Xunit;
using static Utilities.CollectionUtils;

namespace SocialNetwork.UnitTests
{
    public class PostsRepositoryTest
    {
        private readonly SqliteConnection _dbConnection;
        private readonly SocialNetworkDbContext _dbContext;
        private readonly IPostsRepository _postsRepo;
        private readonly IRepository<User> _usersRepo;
        private readonly TestDataContainer _testDataContainer;

        public PostsRepositoryTest()
        {
            // arange
            var (dbContext, connection) = TestUtils.InitDbInMemory().Result;
            _dbConnection = connection;
            _dbContext = dbContext;
            _postsRepo = new PostsRepository(_dbContext.Posts);
            _usersRepo = new Repository<User>(_dbContext.Users);

            _testDataContainer = new TestDataContainer();
        }

        private async Task SaveData(IEnumerable<User> users, IEnumerable<Post> posts)
        {
            users.ForEach(it => _dbContext.Users.Add(it));
            await _dbContext.SaveChangesAsync();
            posts.ForEach(it => _postsRepo.Insert(it));
            await _dbContext.SaveChangesAsync();
        }

        private void Cleenup()
        {
            _dbConnection?.Close();
        }

        [Fact]
        public async Task Write()
        {
            // arange
            var usersToSave = _testDataContainer.Users.Values;
            var postsToSave = _testDataContainer.Posts;
            var postsToSave_ids = postsToSave.Select(it => it.Id);

            // act
            await SaveData(usersToSave, postsToSave);

            // assert
            var loadedPosts = await _postsRepo.GetMany();
            loadedPosts
                .All(it => postsToSave_ids.Contains(it.Id))
                .Also(Assert.True);

            // cleanup
            Cleenup();
        }

        [Fact]
        public async Task Repository_Insert_FirstWithIdsThanWithout()
        {
            // arange
            long lastGeneratedId = 0;
            var usersToSave = _testDataContainer.Users.Values;
            IEnumerable<Post> postsWithIds =
                NewArray(() => Generator.GeneratePost(id: ++lastGeneratedId), 10);
            var postsWithoutIds = _testDataContainer.Posts;

            // act
            await SaveData(usersToSave, postsWithIds);
            postsWithoutIds.ForEach(_postsRepo.Insert);
            await _dbContext.SaveChangesAsync();

            // cleanup
            Cleenup();
        }

        [Fact]
        public async Task Repository_Insert_MixWithAndWithoutIds()
        {
            // arange
            var usersToSave = _testDataContainer.Users.Values;

            // act
            usersToSave.ForEach(_usersRepo.Insert);
            await _dbContext.SaveChangesAsync();

            _postsRepo.Insert(Generator.GeneratePost(id: 1));
            _postsRepo.Insert(Generator.GeneratePost());
            _postsRepo.Insert(Generator.GeneratePost(id: 3));
            _postsRepo.Insert(Generator.GeneratePost());
            await _dbContext.SaveChangesAsync();

            var savedPosts =
                await _postsRepo
                    .GetMany(selector: it => it.Id);

            savedPosts.SequenceEqual(new long[] {1, 2, 3, 4});

            // cleanup
            Cleenup();
        }

        [Fact]
        public async Task OrderBy()
        {
            // arange
            var usersToSave = _testDataContainer.Users.Values;
            IOrderedEnumerable<Post> postsOrderedByDate = _testDataContainer.Posts
                .Where(it => it.Id != default)
                .OrderByDescending(it => it.CreatedAt);
            IOrderedEnumerable<Post> schuffledPosts = postsOrderedByDate.Schuffle();

            // act
            await SaveData(usersToSave, schuffledPosts);

            // prepare assert assert
            var loadedPostsOrderedByDate = await _postsRepo.GetMany(order: PostsOrder.CreatedAtDescending);

            // assert
            loadedPostsOrderedByDate.SequenceEqual(postsOrderedByDate).Also(Assert.True);

            // cleanup
            Cleenup();
        }

        [Fact]
        public async Task Update()
        {
            // arange
            var usersToSave = _testDataContainer.Users.Values;
            var postsToSave = _testDataContainer.Posts;
            long postToUpdateId = 3;
            var textToSet = "trallalalalla";

            // act
            await SaveData(usersToSave, postsToSave);
            await _postsRepo.Update(postToUpdateId, it => it.Text = textToSet);
            await _dbContext.SaveChangesAsync();

            // assert
            Post updatedPost = await _postsRepo.GetOne(it => it.Id == postToUpdateId);
            Assert.True(updatedPost.Text == textToSet);

            // cleanup
            Cleenup();
        }

        [Fact]
        public async Task Update2()
        {
            // arange
            var usersToSave = _testDataContainer.Users.Values;
            var postsToSave = _testDataContainer.Posts;
            long postToUpdateId = 3;
            var textToSet = "trallalalalla";

            // act
            await SaveData(usersToSave, postsToSave);
            Post postToUpdate = await _postsRepo.GetOne(postToUpdateId);
            postToUpdate.Text = textToSet;
            await _dbContext.SaveChangesAsync();

            // assert
            Post updatedPost = await _postsRepo.GetOne(postToUpdateId);
            Assert.True(updatedPost.Text == textToSet);

            // cleanup
            Cleenup();
        }

        [Fact]
        public async Task Delete()
        {
            // arange
            var usersToSave = _testDataContainer.Users.Values;
            var postsToSave = _testDataContainer.Posts;
            long postToDeleteId = 3;

            // act
            await SaveData(usersToSave, postsToSave);
            await _postsRepo.Delete(postToDeleteId);
            await _dbContext.SaveChangesAsync();

            // assert
            (await _dbContext.Posts.AnyAsync(it => it.Id == 3)).Also(Assert.False);

            // cleanup
            Cleenup();
        }

        [Fact]
        public async Task ReadPostAuthor()
        {
            // arange
            var usersToSave = _testDataContainer.Users.Values;
            var postsToSave = _testDataContainer.Posts;

            // act
            await SaveData(usersToSave, postsToSave);

            // prepare assert
            IList<Post> posts = await _postsRepo.GetMany(propsToInclude: "Author");
            posts.Select(it => it.Author).ForEach(Assert.NotNull);
        }

        [Fact]
        public async Task TestPostRatings()
        {
            long lastGeneratedPostId = 0;
            int lastGeneratedUserId = 0;

            // arange
            var usersToSave = _testDataContainer.Users.Values.ToArray();
            var postsToSave = _testDataContainer.Posts;
            
            usersToSave.ForEach(it => it.Id = (++lastGeneratedUserId).ToString());
            postsToSave.ForEach(it => it.Id = ++lastGeneratedPostId);

            PostRating NewPostRating(long postId, string userId) => new PostRating()
            {
                PostId = postId,
                UserId = userId,
                RatingType = Generator.RandomEnumValue<PostRating.Type>()
            };

            var ratingsToSave = new []
            {
                NewPostRating(1, "1"),
                NewPostRating(2, "1"),
                NewPostRating(1, "4"),
                NewPostRating(3, "1"),
                NewPostRating(4, "1"),
                NewPostRating(2, "4"),
                NewPostRating(3, "2"),
                NewPostRating(4, "3"),
                NewPostRating(4, "2"),
            };

            await SaveData(usersToSave, postsToSave);
            ratingsToSave.ForEach(_dbContext.PostRatings.Add);

            await _dbContext.SaveChangesAsync();

            async Task AssertThatPostWasRatedByUsers(long postId, params string[] userIds)
            {
                var post = await _postsRepo.GetOne(postId);
                var post1RaterIds = post.Ratings.Select(it => it.UserId);
                Assert.True(post1RaterIds.ContainsAll(userIds));
            }

            async Task AssertThatPostWasntRatedByUsers(long postId, params string[] userIds)
            {
                var post = await _postsRepo.GetOne(postId);
                var post1RatersIds = post.Ratings.Select(it => it.UserId);
                Assert.True(post1RatersIds.All(it => !userIds.Contains(it)));
            }

            await AssertThatPostWasRatedByUsers(1, "1", "4");
            await AssertThatPostWasRatedByUsers(4, "2", "3", "1");

            await AssertThatPostWasntRatedByUsers(3, "3", "4");
        }

        [Fact]
        public async Task ReadPostRaters()
        {
            long lastGeneratedPostId = 0;
            int lastGeneratedUserId = 0;

            // arange
            var usersToSave = _testDataContainer.Users.Values.ToArray();
            var postsToSave = _testDataContainer.Posts;

            usersToSave.ForEach(it => it.Id = (++lastGeneratedUserId).ToString());
            postsToSave.ForEach(it => it.Id = ++lastGeneratedPostId);

            PostRating NewPostRating(long postId, string userId) => new PostRating()
            {
                PostId = postId,
                UserId = userId,
                RatingType = Generator.RandomEnumValue<PostRating.Type>()
            };

            var ratingsToSave = new[]
            {
                NewPostRating(1, "1"),
                NewPostRating(1, "4"),
                NewPostRating(2, "1"),
                NewPostRating(2, "4"),
                NewPostRating(3, "1"),
                NewPostRating(3, "2"),
                NewPostRating(4, "1"),
                NewPostRating(4, "2"),
                NewPostRating(4, "3"),
            };

            await SaveData(usersToSave, postsToSave);
            ratingsToSave.ForEach(_dbContext.PostRatings.Add);

            await _dbContext.SaveChangesAsync();

            var allSavedPosts = await _postsRepo.GetMany();

            allSavedPosts.ForEach(it => it.Ratings.Select(rating => rating.User));

            var post4Raters = allSavedPosts.Single(it => it.Id == 4).Ratings.Select(it => it.User);
            post4Raters.Select(it => it.Id).ContainsAll(new[] {"1", "2", "3"}).Also(Assert.True);
        }
    }
}
