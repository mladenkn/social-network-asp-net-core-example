using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Interfaces.Services;

namespace SocialNetwork.Web.Services
{
    public class Hub : IHub
    {
        private readonly IHubContext<Microsoft.AspNetCore.SignalR.Hub> _wrapee;

        public Hub(IHubContext<Microsoft.AspNetCore.SignalR.Hub> wrapee)
        {
            _wrapee = wrapee;
        }

        public Task Emit(string name, object data) => _wrapee.Clients.All.SendAsync(name, data);
    }
}
