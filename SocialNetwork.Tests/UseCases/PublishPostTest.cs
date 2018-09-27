using FluentAssertions;
using SocialNetwork.Domain.Posts;
using SocialNetwork.Domain.UseCases;
using SocialNetwork.Domain.Users;
using SocialNetwork.Tests.Abstract;
using System.Linq;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using Xunit;

namespace SocialNetwork.Tests.UseCases
{
    public class PublishPostTest : IntegrationTestBase
    {
        [Fact]
        public async Task Should_succeed_saving_post_in_db()
        {
            Mock<GetCurrentUserId>(() => "");
            Initialize();

            var postText = Faker.Lorem.Text();

            var request = new PublishPost.Request { Text = postText };
            var response = await SendRequest(request);

            response.Should()
                .BeAssignableTo<Response<Post>>().Which
                .Payload.Text.Should().Be(postText);

            Query<Post>().Count(p => p.Text == postText).Should().Be(1);
        }
    }
}
