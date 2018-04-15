using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Interface.Services;

namespace SocialNetwork.Services
{
    public class PostsHub : IPostsHub
    {
        private readonly IHubContext<Microsoft.AspNetCore.SignalR.Hub> _wrapee;

        public PostsHub(IHubContext<Microsoft.AspNetCore.SignalR.Hub> wrapee)
        {
            _wrapee = wrapee;
        }

        public Task Emit(string name, object data) => _wrapee.Clients.All.SendAsync(name, data);
    }
}
