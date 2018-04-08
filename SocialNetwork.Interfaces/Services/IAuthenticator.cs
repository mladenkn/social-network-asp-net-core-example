using System.Threading.Tasks;
using SocialNetwork.Interface.Models;
using SocialNetwork.Interface.Models.Entities;
using SignInResult = SocialNetwork.Interface.Models.SignInResult;

namespace SocialNetwork.Interface.Services
{
    public interface IAuthenticator
    {
        Task<RegistrationResult> Register(User user, string password);
        Task<SignInResult> SignIn(string username, string password, bool isPersistent = true);
        Task SignOut();
    }
}
