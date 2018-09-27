using SocialNetwork.Domain.Users;
using System.Linq;

namespace SocialNetwork.DevelopmentUtilities
{
    public class CurrentUserAccessor
    {
        private readonly Settings _settings;
        private readonly IQueryable<User> _users;

        public CurrentUserAccessor(Settings settings, IQueryable<User> users)
        {
            _settings = settings;
            _users = users;
        }

        public string GetCurrentUserId() => _users.FirstOrDefault(u => u.UserName == _settings.CurrentUserUserName).Id;
    }
}
