using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Posts;
using SocialNetwork.Domain.Users;
using SocialNetwork.Domain.PostRatings;
using Xunit;

namespace SocialNetwork.Tests.Database
{
    public class PostWrites : DatabaseTest
    {
        [Fact]
        public void Should_fail_to_save_without_required_properties()
        {
            _db.Posts.Add(new Post());
            Func<Task> f = async () => await _db.SaveChangesAsync();
            f.Should().ThrowExactly<DbUpdateException>();
        }
    }
}
