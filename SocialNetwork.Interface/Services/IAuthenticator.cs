using System.Threading.Tasks;
using SocialNetwork.Interface.Models;
using SocialNetwork.Interface.Models.Entities;

namespace SocialNetwork.Interface.Services
{
    public interface IAuthenticator
    {
        Task<Result> Register(User user, string password);
        Task<Result> SignIn(string username, string password, bool isPersistent = true);
        Task SignOut();
    }
}
