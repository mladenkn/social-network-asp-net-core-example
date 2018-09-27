using FluentAssertions;
using SocialNetwork.Domain.Posts;
using SocialNetwork.Domain.UseCases;
using SocialNetwork.Domain.Users;
using SocialNetwork.Tests.Abstract;
using System.Threading.Tasks;
using Xunit;

namespace SocialNetwork.Tests.UseCases
{
    public class DeletePostTest : IntegrationTestBase
    {
        [Fact]
        public async Task Should_succeed_deleting_Post_from_db()
        {
            Mock<GetCurrentUserId>(() => "1");
            Initialize();

            var post = Generate<Post>().RuleFor(p => p.AuthorId, "1").Generate();
            await SaveToDatabase(post);

            var response = await SendRequest(new DeletePost.Request { PostId = post.Id });

            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Should_fail_when_post_doesnt_exist_in_db()
        {
            Mock<GetCurrentUserId>(() => "");
            Initialize();

            var response = await SendRequest(new DeletePost.Request { PostId = 23 });

            response.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task Should_fail_when_user_is_trying_to_delete_someone_elses_post()
        {
            Mock<GetCurrentUserId>(() => "1");
            Initialize();

            var post = Generate<Post>().RuleFor(p => p.AuthorId, "2").Generate();
            await SaveToDatabase(post);

            var response = await SendRequest(new DeletePost.Request { PostId = 23 });

            response.IsSuccess.Should().BeFalse();
        }
    }
}
