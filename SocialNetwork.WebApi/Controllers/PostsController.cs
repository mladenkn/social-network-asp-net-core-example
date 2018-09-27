using System.Threading.Tasks;
using ApplicationKernel.Domain.DataQueries;
using ApplicationKernel.Infrastructure.WebApi;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Domain.UseCases;
using SocialNetwork.Domain.Posts;

namespace SocialNetwork.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ApiController
    {
        public PostsController(HandleApiRequest handleApiRequest) : base(handleApiRequest)
        {
        }

        [HttpGet]
        public Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10, string authorId = null)
        {
            var query = new GetPosts.Request
            {
                AuthorId = authorId,
                Paging = new Paging(pageNumber, pageSize),
                Order = OrderBuilder.Of<Post>().Descending(p => p.CreatedAt).Build()
            };

            return HandleRequest(query);
        }

        [HttpPost]
        public Task<IActionResult> Post(PublishPost.Request command) => HandleRequest(command);

        [HttpPut]
        public Task<IActionResult> Put(EditPost.Request command) => HandleRequest(command);

        [HttpDelete("{id}")]
        public Task<IActionResult> Delete(long id) => HandleRequest(new DeletePost.Request { PostId = id });
    }
}
