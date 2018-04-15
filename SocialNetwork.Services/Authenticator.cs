using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Interface.Models;
using SocialNetwork.Interface.Models.Entities;
using SocialNetwork.Interface.Services;
using Utilities;
using SignInResult = SocialNetwork.Interface.Models.SignInResult;

namespace SocialNetwork.Services
{
    public class Authenticator : IAuthenticator
    {
        private readonly UserManager<User> _manager;
        private readonly SignInManager<User> _signInManager;

        public Authenticator(UserManager<User> manager, SignInManager<User> signInManager)
        {
            _manager = manager;
            _signInManager = signInManager;
        }

        public Task<RegistrationResult> Register(User user, string password)
        {
            return _manager
                .CreateAsync(user, password)
                .Map(it =>
                {
                    if (it.Succeeded)
                        return (RegistrationResult) new RegistrationSuccess();
                    else
                    {
                        if (it.Errors.Any(e => e.Code == "DuplicateUserName"))
                            return new RegistrationFailure(RegistrationError.DuplicateUserName);
                        else
                            return new RegistrationFailure();
                    }
                });
        }

        public Task<SignInResult> SignIn(string username, string password, bool isPersistent = true)
        {
            return _signInManager
                .PasswordSignInAsync(username, password, isPersistent: isPersistent, lockoutOnFailure: false)
                .Map(it => it.Succeeded ? (SignInResult) new SignInSuccess() : new SignInFailure());
        }

        public Task SignOut() => _signInManager.SignOutAsync();
    }
}
