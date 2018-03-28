using System.Collections.Generic;

namespace SocialNetwork.Models
{
    public class User : IEntity<long>
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string ProfileImageUrl { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
