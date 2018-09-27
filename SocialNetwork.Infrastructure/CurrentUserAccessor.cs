using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SocialNetwork.Domain.Users;

namespace SocialNetwork.Infrastructure
{
    public class CurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IQueryable<User> _users;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor, IQueryable<User> users)
        {
            _httpContextAccessor = httpContextAccessor;
            _users = users;
        }

        public string GetId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
