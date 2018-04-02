using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.DAL;
using SocialNetwork.Interfaces.Models;
using SocialNetwork.Interfaces.Services;
using SocialNetwork.Models;
using SignInResult = SocialNetwork.Interfaces.Models.SignInResult;

namespace SocialNetwork.Services
{
    public class UserManager : Repository<User>, IUserManager
    {
        private readonly UserManager<User> _manager;
        private readonly SignInManager<User> _signInManager;

        public UserManager(UserManager<User> manager, DbSet<User> usersDbSet, SignInManager<User> signInManager) : base(usersDbSet)
        {
            _manager = manager;
            _signInManager = signInManager;
        }

        public Task<RegistrationResult> Register(User user, string password)
        {
            return _manager
                .CreateAsync(user, password)
                .Map(it => new RegistrationResult(it.Succeeded));
        }

        public Task<SignInResult> SignIn(string username, string password, bool isPersistent)
        {
            return _signInManager
                .PasswordSignInAsync(username, password, isPersistent: isPersistent, lockoutOnFailure: false)
                .Map(it => new SignInResult(it.Succeeded));
        }

        public Task SignOut() => _signInManager.SignOutAsync();
    }
}
