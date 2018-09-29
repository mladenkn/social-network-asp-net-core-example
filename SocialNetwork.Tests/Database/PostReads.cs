using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Posts;
using SocialNetwork.Domain.Users;
using Xunit;

namespace SocialNetwork.Tests.Database
{
    public class PostReads : DatabaseTest
    {
        [Fact]
        public async Task Should_get_single_by_id()
        {
            var user = new User {UserName = "Mladen", ProfileImageUrl = ""};
            _db.Users.Add(user);

            var post = new Post
            {
                AuthorId = user.Id,
                Text = "tralalalalall",
                CreatedAt = DateTime.Now
            };
            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            var postFromProvider = await _db.Posts.FirstOrDefaultAsync(it => it.Id == post.Id);

            postFromProvider.Id.Should().Be(post.Id);
        }
    }
}
