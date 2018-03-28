using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Web.ServiceInterfaces;

namespace SocialNetwork.Web.Services
{
    public class PostsHub : Hub, IPostsHub
    {
        public Task EmitPostPublished(string postHtml)
        {
            return Clients.All.SendAsync(Constants.PostsEvents.PostPublished, postHtml);
        }

        public Task EmitPostChanged(string postHtml)
        {
            return Clients.All.SendAsync(Constants.PostsEvents.PostChanged, postHtml);
        }

        public Task EmitPostDeleted(long postId)
        {
            return Clients.All.SendAsync(Constants.PostsEvents.PostDeleted, postId);
        }
    }
}
