using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Interfaces.Services;
using SocialNetwork.Models;

namespace SocialNetwork.Web.Services
{
    public class UserManager : IUserManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<User> _usersRepository;

        public UserManager(UserManager<User> userManager, IRepository<User> usersRepository)
        {
            _userManager = userManager;
            _usersRepository = usersRepository;
        }

        public Task<User> GetByUsernameAsync(string username) => _usersRepository.GetOne(it => it.UserName == username);
    }
}
