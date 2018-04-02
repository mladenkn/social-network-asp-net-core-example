using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.DAL;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Interfaces.Services;
using SocialNetwork.Models;

namespace SocialNetwork.Services
{
    public class UserManager : Repository<User>, IUserManager
    {
        private readonly UserManager<User> _manager;

        public UserManager(UserManager<User> manager, DbSet<User> usersDbSet) : base(usersDbSet)
        {
            _manager = manager;
        }
    }
}
