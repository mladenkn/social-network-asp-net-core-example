using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Interfaces.Services;
using SocialNetwork.Models;

namespace SocialNetwork.Web.Services
{
    public class UserManager : IUserManager
    {
        private readonly UserManager<User> _manager;
        private readonly IRepository<User> _repo;

        public UserManager(UserManager<User> manager, IRepository<User> repo)
        {
            _manager = manager;
            _repo = repo;
        }

        public Task<User> GetOneByUsernameAsync(string username) => _repo.GetOne(it => it.UserName == username);

        public Task<IList<User>> GetMany(Expression<Func<User, bool>> filter = null,
            Expression<Func<IQueryable<User>, IOrderedQueryable<User>>> orderBy = null,
            int? count = null, int skip = 0,
            params string[] propsToInclude) => _repo.GetMany(filter, orderBy, count, skip);

        public Task<User> GetOne(Expression<Func<User, bool>> selector = null, params string[] propsToInclude) =>
            _repo.GetOne(selector, propsToInclude);

        public Task<User> Update(Expression<Func<User, bool>> selector, Action<User> consumeItem,
            params string[] propsToInclude) =>
            _repo.Update(selector, consumeItem, propsToInclude);

        public User Update(User post) => _repo.Update(post);

        public User Insert(User entity) => _repo.Insert(entity);

        public Task Delete(Expression<Func<User, bool>> filter) => _repo.Delete(filter);
    }
}
