using FluentAssertions;
using SocialNetwork.Domain.Posts;
using SocialNetwork.Domain.UseCases;
using SocialNetwork.Domain.Users;
using SocialNetwork.Tests.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using Utilities;
using Xunit;

namespace SocialNetwork.Tests.UseCases
{
    public class EditPostTest : IntegrationTestBase
    {
        [Fact]
        public async Task Should_succeed_updating_post_in_db()
        {
            Mock<GetCurrentUserId>(() => "1");
            Initialize();

            Generate<Post>()
                .RuleFor(p => p.Text, f => f.Lorem.Text())
                .RuleFor(p => p.AuthorId, "1")
                .Generate()
                .SaveTo(out var post);

            await SaveToDatabase(post);

            var newText = Faker.Lorem.Text();

            var response = await SendRequest(new EditPost.Request { PostId = post.Id, NewText = newText });

            response.IsSuccess.Should().BeTrue();
            response.Should().BeAssignableTo<Response<Post>>().Which.Payload.Text.Should().Be(newText);
            Query<Post>().Count(p => p.Text == newText).Should().Be(1);
        }

        [Fact]
        public async Task Should_fail_when_post_doesnt_exits_in_db()
        {
            Mock<GetCurrentUserId>(() => "");
            Initialize();

            var newText = Faker.Lorem.Text();

            var response = await SendRequest(new EditPost.Request { PostId = 234, NewText = "fsdfsd" });

            response.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task Should_fail_when_user_is_trying_to_edit_someone_elses_post()
        {
            Mock<GetCurrentUserId>(() => "1");
            Initialize();

            Generate<Post>()
                .RuleFor(p => p.Text, f => f.Lorem.Text())
                .RuleFor(p => p.AuthorId, "2")
                .Generate()
                .SaveTo(out var post);

            await SaveToDatabase(post);

            var newText = Faker.Lorem.Text();
            var response = await SendRequest(new EditPost.Request { PostId = post.Id, NewText = newText });

            response.IsSuccess.Should().BeFalse();
        }
    }
}
