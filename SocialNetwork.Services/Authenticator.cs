using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Interface.Models;
using SocialNetwork.Interface.Models.Entities;
using SocialNetwork.Interface.Services;
using Utilities;

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

        public Task<Result> Register(User user, string password)
        {
            return _manager
                .CreateAsync(user, password)
                .Map(it =>
                {
                    if (it.Succeeded)
                        return (Result) new Success();
                    else
                    {
                        if (it.Errors.Any(e => e.Code == "DuplicateUserName"))
                            return new Failure<RegistrationError>(RegistrationError.DuplicateUserName);
                        else
                            return new Failure<RegistrationError>();
                    }
                });
        }

        public Task<Result> SignIn(string username, string password, bool isPersistent = true)
        {
            return _signInManager
                .PasswordSignInAsync(username, password, isPersistent: isPersistent, lockoutOnFailure: false)
                .Map(it => it.Succeeded ? (Result) new Success() : new Failure<RegistrationError>());
        }

        public Task SignOut() => _signInManager.SignOutAsync();
    }
}
