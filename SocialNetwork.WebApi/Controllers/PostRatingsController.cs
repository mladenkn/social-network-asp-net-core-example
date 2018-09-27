using System.Threading.Tasks;
using ApplicationKernel.Infrastructure.WebApi;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Domain.UseCases;

namespace SocialNetwork.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostRatingsController : ApiController
    {
        public PostRatingsController(HandleApiRequest handleApiRequest) : base(handleApiRequest)
        {
        }

        [HttpPost]
        public Task<IActionResult> Post(RatePost.Request command) => HandleRequest(command);

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var command = new UnratePost.Request { PostRatingId = id };
            return await HandleRequest(command);
        }
    }
}
